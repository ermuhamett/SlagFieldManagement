namespace SlagFieldManagement.Domain.Interfaces;

public interface IDomainEvent
{
    Guid EventId { get; }
    Guid AggregateId { get; }
    string EventType { get; }
    string EventData { get; }
    int Version { get; }
    DateTime Timestamp { get; }
    string Metadata { get; }
}