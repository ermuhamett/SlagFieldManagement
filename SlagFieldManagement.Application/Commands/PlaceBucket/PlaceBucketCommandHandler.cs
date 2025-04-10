using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;
using SlagFieldManagement.Domain.Aggregates.SlagFieldState;
using SlagFieldManagement.Domain.Exceptions;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Commands.PlaceBucket;

public sealed class PlaceBucketCommandHandler:ICommandHandler<PlaceBucketCommand>
{
    private readonly ISlagFieldStateRepository _stateRepository;
    private readonly ISlagFieldPlaceRepository _placeRepository;
    private readonly IBucketRepository _bucketRepository;
    private readonly IStateEventStore _stateEventStore;
    private readonly IUnitOfWork _unitOfWork;
    
    public PlaceBucketCommandHandler(
        ISlagFieldStateRepository stateRepository, 
        ISlagFieldPlaceRepository placeRepository,
        IBucketRepository bucketRepository,
        IStateEventStore stateEventStore,
        IUnitOfWork unitOfWork)
    {
        _stateRepository = stateRepository;
        _placeRepository = placeRepository;
        _bucketRepository = bucketRepository;
        _unitOfWork = unitOfWork;
        _stateEventStore = stateEventStore;
    }
    
    public async Task<Result> Handle(
        PlaceBucketCommand request, 
        CancellationToken ct)
    {
        var place = await _placeRepository.GetByIdAsync(request.PlaceId, ct);
        if (place is null || !place.IsEnable)
            return Result.Failure<Guid>(SlagFieldPlaceErrors.PlaceNotFoundOrDisabled(request.PlaceId));
        
        var bucket = await _bucketRepository.GetByIdAsync(request.BucketId, ct);
        if (bucket is null || bucket.IsDelete)
            return Result.Failure(SlagFieldErrors.BucketNotFound(request.BucketId));
        
        // 2. Получение текущего состояния
        // Проверка текущего состояния места
        var activeState = await _stateRepository.GetActiveStateAsync(request.PlaceId, ct);
        if (activeState != null &&
            activeState.State != StateFieldType.BucketRemoved &&
            activeState.State != StateFieldType.Invalid)
        {
            return Result.Failure<Guid>(SlagFieldStateErrors.PlaceOccupied(request.PlaceId)); 
        }
        
        // 3. Создание нового состояния
        var newState = SlagFieldState.CreateState(request.PlaceId);
        
        newState.PlaceBucket(
            request.BucketId,
            request.MaterialId,
            request.SlagWeight * 1000,
            request.StartDate);

        // 4. Сохранение
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await _stateRepository.AddAsync(newState,ct);
            // Сохраняем все события, сгенерированные агрегатом
            foreach (var @event in newState.Events)
            {
                await _stateEventStore.SaveEventAsync(@event, newState.Id, expectedVersion: 0);
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