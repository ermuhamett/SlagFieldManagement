namespace SlagFieldManagement.Domain.Abstractions;

public abstract record DomainEvent(
    Guid EventId,
    Guid AggregateId,
    string EventType,
    int Version,
    DateTime Timestamp,
    string Metadata
)
{
    // Конструктор для упрощения создания событий
    protected DomainEvent(Guid aggregateId, string eventType, string metadata = "{}")
        : this(
            EventId: Guid.NewGuid(),
            AggregateId: aggregateId,
            EventType: eventType,
            Version: 1, // Версия по умолчанию
            Timestamp: DateTime.UtcNow, 
            Metadata: metadata
        )
    {
    }
}