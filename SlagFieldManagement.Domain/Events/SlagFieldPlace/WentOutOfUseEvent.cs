using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Domain.Events.SlagFieldPlace;

public record WentOutOfUseEvent(
    Guid EventId,
    Guid AggregateId,
    string EventType,
    DateTime Timestamp
) : IDomainEvent
{
    public string EventData => string.Empty;
    public int Version { get; init; } = 1;
    public string Metadata { get; init; } = string.Empty;
}