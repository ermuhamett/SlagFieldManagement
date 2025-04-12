using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldState;
using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Domain.Exceptions;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Commands.Invalid;

public sealed class InvalidCommandHandler:ICommandHandler<InvalidCommand>
{
    private readonly ISlagFieldStateRepository _stateRepository;
    private readonly ISlagFieldStockRepository _stockRepository;
    private readonly IStateEventStore _stateEventStore;
    private readonly IUnitOfWork _unitOfWork;
    
    public InvalidCommandHandler(
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
        InvalidCommand request, 
        CancellationToken ct)
    {
        // Получаем текущее активное состояние
        var currentState = await _stateRepository.GetActiveStateAsync(request.PlaceId, ct);
        if (currentState == null)
        {
            return Result.Failure(SlagFieldStateErrors.NoActiveState(request.PlaceId));
        }
        
        // Проверяем наличие связанной записи в SlagFieldStock
        var stock = await _stockRepository.GetByStateIdAsync(currentState.Id, ct);
        bool stockUpdated = false;
        if (stock != null && !stock.IsDelete)
        {
            var markResult = stock.MarkAsDeleted();
            if (markResult.IsFailure)
            {
                return Result.Failure(markResult.Error);
            }
            stockUpdated = true;
        }
        
        currentState.SetInvalid(request.Description);

        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await _stateRepository.UpdateAsync(currentState, ct);
            if (stockUpdated)
            {
                await _stockRepository.UpdateAsync(stock, ct);
            }
            foreach (var @event in currentState.Events)
            {
                await _stateEventStore.SaveEventAsync(@event, currentState.Id, expectedVersion: 0);
            }
            currentState.ClearEvents();
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