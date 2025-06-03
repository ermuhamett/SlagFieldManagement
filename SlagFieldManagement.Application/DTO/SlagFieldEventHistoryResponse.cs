namespace SlagFieldManagement.Application.DTO;

public record SlagFieldEventHistoryResponse
{
    public Guid   EventId   { get; init; }
    public Guid   PlaceId   { get; init; }
    public string EventType { get; init; }
    public DateTime Timestamp { get; init; }

    // Параметры, актуальные только для state‑событий
    public Guid?   BucketId          { get; init; }
    public Guid?   MaterialId        { get; init; }
    public decimal? SlagWeight       { get; init; }
    public DateTime? ClientStartDate { get; init; }
    public DateTime? BucketEmptiedTime { get; init; }
    public string? Description       { get; init; }
}