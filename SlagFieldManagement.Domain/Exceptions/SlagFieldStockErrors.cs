using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Domain.Exceptions;

public static class SlagFieldStockErrors
{
    public static Error InvalidTotal => new(
        "SlagFieldStock.InvalidTotal",
        "Количество шлака должно быть положительным.");

    public static Error InvalidTransactionType => new(
        "SlagFieldStock.InvalidTransactionType",
        "Недопустимый тип транзакции. Допустимые значения: 'Incoming', 'OutGoing'.");

    public static Error StockNotFound(Guid stockId) => new(
        "SlagFieldStock.NotFound",
        $"Транзакция с Id {stockId} не найдена.");

    public static Error AlreadyDeleted(Guid stockId) => new(
        "SlagFieldStock.AlreadyDeleted",
        $"Транзакция с Id {stockId} уже удалена.");
}