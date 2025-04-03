namespace SlagFieldManagement.Domain.Interfaces;

public interface IDomainEvent
{
    Guid EventId { get; }      // Уникальный идентификатор события
    Guid AggregateId { get; }  // Идентификатор агрегата (PlaceId или StateId)
    string EventType { get; }  // Тип события (например, "PlaceCreated")
    string EventData { get; }  // Данные события в формате строки (обычно JSON)
    int Version { get; }       // Версия события
    DateTime Timestamp { get; } // Время создания события
    string Metadata { get; }   // Дополнительные метаданные
}