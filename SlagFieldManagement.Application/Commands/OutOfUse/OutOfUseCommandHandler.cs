using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;
using SlagFieldManagement.Domain.Aggregates.SlagFieldState;
using SlagFieldManagement.Domain.Exceptions;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Commands.OutOfUse;

public sealed class OutOfUseCommandHandler:ICommandHandler<OutOfUseCommand>
{
    private readonly ISlagFieldPlaceRepository _placeRepository;
    private readonly ISlagFieldStateRepository _stateRepository;
    private readonly IPlaceEventStore _placeEventStore;
    private readonly IStateEventStore _stateEventStore;
    private readonly IUnitOfWork _unitOfWork;

    public OutOfUseCommandHandler(
        ISlagFieldPlaceRepository placeRepository,
        ISlagFieldStateRepository stateRepository,
        IPlaceEventStore placeEventStore,
        IStateEventStore stateEventStore,
        IUnitOfWork unitOfWork)
    {
        _placeRepository = placeRepository;
        _stateRepository = stateRepository;
        _placeEventStore = placeEventStore;
        _stateEventStore = stateEventStore;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        OutOfUseCommand request, 
        CancellationToken ct)
    {
        var place = await _placeRepository.GetByIdAsync(request.PlaceId, ct);
        if (place is null || !place.IsEnable)
            return Result.Failure(SlagFieldPlaceErrors.AlreadyDisabled(request.PlaceId));

        var activeState = await _stateRepository.GetActiveStateAsync(request.PlaceId, ct);
        if (activeState != null)
        {
            activeState.SetInvalid("Место деактивировано через команду OutOfUse");
        }

        // Пытаемся деактивировать место
        var disableResult = place.Disable();
        if (disableResult.IsFailure)
        {
            return Result.Failure(disableResult.Error);
        }
        
        // Сохраняем изменения в транзакции
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            // Сохраняем обновление состояния, если оно было
            if (activeState != null)
            {
                _stateRepository.Update(activeState);
                foreach (var @event in activeState.Events)
                {
                    await _stateEventStore.SaveEventAsync(@event, activeState.Id, expectedVersion: 0);
                }
                activeState.ClearEvents();
            }
            // Сохраняем обновление места
            _placeRepository.Update(place);
            foreach (var @event in place.Events)
            {
                await _placeEventStore.SaveEventAsync(@event, place.Id, expectedVersion: 0);
            }
            place.ClearEvents();
            
            await _unitOfWork.SaveChangesAsync(ct);
            await _unitOfWork.CommitTransactionAsync(ct);
            return Result.Success();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(ct);
            throw; 
        }
    }
}