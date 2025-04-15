using System.Text.Json;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Domain.Events.SlagFieldState;

public record BucketEmptiedEvent(
    Guid EventId,
    Guid AggregateId,
    string EventType,
    DateTime Timestamp,
    DateTime BucketEmptiedTime
) : IDomainEvent
{
    public string EventData => JsonSerializer.Serialize(new{BucketEmptiedTime});
    public int Version { get; init; } = 1;
    public string Metadata { get; init; } = string.Empty;
}