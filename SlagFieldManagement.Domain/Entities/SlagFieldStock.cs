using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Exceptions;

namespace SlagFieldManagement.Domain.Entities;

public sealed class SlagFieldStock:Entity
{
    public Guid? SlagFieldStateId { get; private set; }
    public Guid MaterialId { get; private set; }
    public decimal Total { get; private set; }
    public Guid? ApplicationId { get; private set; }
    public string TransactionType { get; private set; }
    public bool IsDelete { get; private set; }
    
    private SlagFieldStock(
        Guid id,
        Guid? slagFieldStateId,
        Guid materialId,
        decimal total,
        Guid? applicationId,
        string transactionType) : base(id)
    {
        SlagFieldStateId = slagFieldStateId;
        MaterialId = materialId;
        Total = total;
        ApplicationId = applicationId;
        TransactionType = transactionType;
        IsDelete = false;
    }

    // Фабричный метод для создания сущности
    public static Result<SlagFieldStock> Create(
        Guid? slagFieldStateId,
        Guid materialId,
        decimal total,
        Guid? applicationId,
        string transactionType)
    {
        // Валидация Total
        if (total <= 0)
            return Result.Failure<SlagFieldStock>(SlagFieldStockErrors.InvalidTotal);

        // Валидация TransactionType
        if (transactionType != "Incoming" && transactionType != "OutGoing")
            return Result.Failure<SlagFieldStock>(SlagFieldStockErrors.InvalidTransactionType);

        // Создание сущности
        var stock = new SlagFieldStock(
            Guid.NewGuid(),
            slagFieldStateId,
            materialId,
            total,
            applicationId,
            transactionType
        );
        
        return Result.Success(stock);
    }
    
    // Метод для логического удаления транзакции
    public Result MarkAsDeleted()
    {
        if (IsDelete)
            return Result.Failure(SlagFieldStockErrors.AlreadyDeleted(Id));

        IsDelete = true;
        return Result.Success();
    }
}