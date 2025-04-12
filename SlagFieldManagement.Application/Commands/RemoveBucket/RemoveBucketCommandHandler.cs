using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;
using SlagFieldManagement.Domain.Aggregates.SlagFieldState;
using SlagFieldManagement.Domain.Exceptions;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Commands.RemoveBucket;

public sealed class RemoveBucketCommandHandler:ICommandHandler<RemoveBucketCommand>
{
    private readonly ISlagFieldStateRepository _stateRepository;
    private readonly ISlagFieldPlaceRepository _placeRepository;
    private readonly IStateEventStore _stateEventStore;
    private readonly IUnitOfWork _unitOfWork;
    
    public RemoveBucketCommandHandler(
        ISlagFieldStateRepository stateRepository,
        ISlagFieldPlaceRepository placeRepository,
        IStateEventStore stateEventStore,
        IUnitOfWork unitOfWork)
    {
        _stateRepository = stateRepository;
        _placeRepository = placeRepository;
        _stateEventStore = stateEventStore;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        RemoveBucketCommand request, 
        CancellationToken ct)
    {
        var place = await _placeRepository.GetByIdAsync(request.PlaceId, ct);
        if (place == null || !place.IsEnable)
        {
            return Result.Failure(SlagFieldPlaceErrors.PlaceNotFoundOrDisabled(request.PlaceId));
        }
        
        var currentState = await _stateRepository.GetActiveStateAsync(request.PlaceId,ct);
        if (currentState == null || currentState.State != StateFieldType.BucketEmptied)
        {
            return Result.Failure(SlagFieldStateErrors.InvalidStateForRemoval(request.PlaceId));
        }

        currentState.RemoveBucket();

        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await _stateRepository.AddAsync(currentState, ct);
            // Сохраняем все события, сгенерированные агрегатом
            foreach (var @event in currentState.Events)
            {
                await _stateEventStore.SaveEventAsync(@event, currentState.Id, expectedVersion: 0);
            }
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