using System.Text.Json;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Domain.Events.SlagFieldState;

public record BucketPlacedEvent(
    Guid EventId,
    Guid AggregateId,
    string EventType,
    DateTime Timestamp,
    Guid BucketId,
    Guid MaterialId,
    decimal SlagWeight
) : IDomainEvent
{
    public string EventData => JsonSerializer.Serialize(new { BucketId, MaterialId, SlagWeight });
    public int Version { get; init; } = 1;
    public string Metadata { get; init; } = string.Empty;
}