namespace SlagFieldManagement.Domain.Interfaces;

public interface IDomainEvent
{
    int EventId { get; }
    int AggregateId { get; }
    string EventType { get; }
    string EventData { get; }
    int Version { get; }
    DateTime Timestamp { get; }
    string Metadata { get; }
}