namespace SlagFieldManagement.Domain.Projection;

public class SlagFieldStateProjection
{
    public Guid PlaceId { get; init; }
    public string Row { get; init; }
    public int Number { get; init; }
    public bool IsEnable { get; init; }
    public Guid? StateId { get; init; }
    public string? State { get; init; } // BucketPlaced, BucketEmptied, NoActiveState или NotInUse
    public Guid? BucketId { get; init; }
    public Guid? MaterialId { get; init; }
    public decimal? SlagWeight { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? Description { get; init; }
}