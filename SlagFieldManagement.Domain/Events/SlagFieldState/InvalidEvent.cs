using System.Text.Json;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Domain.Events;

public record InvalidEvent(
    Guid EventId,
    Guid AggregateId,
    string EventType,
    DateTime Timestamp,
    string Description
) : IDomainEvent
{
    public string EventData => JsonSerializer.Serialize(new { Description });
    public int Version { get; init; } = 1;
    public string Metadata { get; init; } = string.Empty;
}