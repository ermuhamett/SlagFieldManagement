using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;
using SlagFieldManagement.Domain.Exceptions;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Commands.WentInUse;

public class WentInUseCommandHandler:ICommandHandler<WentInUseCommand>
{
    private readonly ISlagFieldPlaceRepository _placeRepository;
    private readonly IPlaceEventStore _placeEventStore;
    private readonly IUnitOfWork _unitOfWork;
    
    public WentInUseCommandHandler(
        ISlagFieldPlaceRepository placeRepository,
        IPlaceEventStore placeEventStore,
        IUnitOfWork unitOfWork)
    {
        _placeRepository = placeRepository;
        _placeEventStore = placeEventStore;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        WentInUseCommand request,
        CancellationToken ct)
    {
        // Получаем агрегат SlagFieldPlace
        var place = await _placeRepository.GetByIdAsync(request.PlaceId, ct);
        if (place == null)
        {
            return Result.Failure(SlagFieldPlaceErrors.PlaceNotFound(request.PlaceId));
        }
        
        // Пытаемся активировать место
        var enableResult = place.Enable();
        if (enableResult.IsFailure)
        {
            return Result.Failure(enableResult.Error);
        }
        
        // Сохраняем изменения в транзакции
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
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