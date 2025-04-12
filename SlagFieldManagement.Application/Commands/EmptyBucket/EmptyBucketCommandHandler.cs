using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldState;
using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Domain.Exceptions;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Commands.EmptyBucket;

public sealed class EmptyBucketCommandHandler:ICommandHandler<EmptyBucketCommand>
{
    private readonly ISlagFieldStateRepository _stateRepository;
    private readonly ISlagFieldStockRepository _stockRepository;
    private readonly IStateEventStore _stateEventStore;
    private readonly IUnitOfWork _unitOfWork;
    
    public EmptyBucketCommandHandler(
        ISlagFieldStateRepository stateRepository,
        ISlagFieldStockRepository stockRepository,
        IStateEventStore stateEventStore,
        IUnitOfWork unitOfWork)
    {
        _stateRepository = stateRepository;
        _stockRepository = stockRepository;
        _stateEventStore = stateEventStore;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        EmptyBucketCommand request, 
        CancellationToken ct)
    {
        // 1. Получение текущего состояния
        var state = await _stateRepository.GetActiveStateAsync(request.PlaceId,ct);
        if (state == null || state.State != StateFieldType.BucketPlaced)
            return Result.Failure(SlagFieldStateErrors.NoBucketToEmpty(request.PlaceId));

        // 2. Опустошение ковша
        state.EmptyBucket(request.EndDate);
        
        // 3. Создание записи в Stock
        var stockEntry = SlagFieldStock.Create(
            state.Id,
            state.MaterialId!.Value,
            state.SlagWeight,
            null,
            "Incoming");
        
       
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await _stockRepository.AddAsync(stockEntry.Value,ct);
            // Сохраняем все события, сгенерированные агрегатом
            foreach (var @event in state.Events)
            {
                await _stateEventStore.SaveEventAsync(@event, state.Id, expectedVersion: 0);
            }
            state.ClearEvents();
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