using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Events;
using SlagFieldManagement.Domain.Events.SlagFieldState;
using SlagFieldManagement.Domain.Exceptions;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Domain.Aggregates.SlagFieldState;

public class SlagFieldState:AggregateBase
{
    public Guid PlaceId { get; private set; }
    public Guid? BucketId { get; private set; }
    public Guid? MaterialId { get; private set; }
    public decimal SlagWeight { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Description { get; private set; }
    public StateFieldType State { get; private set; }
    public bool IsDelete { get; private set; }
    private SlagFieldState(Guid id, Guid placeId) : base(id)
    {
        PlaceId = placeId;
        State = StateFieldType.Initial; 
    }
    public static SlagFieldState CreateState(Guid placeId)
    {
        return new SlagFieldState(Guid.NewGuid(), placeId);
    }
    
    // Метод для установки ковша на место, возможно только если состояние Initial или BucketRemoved
    public void PlaceBucket(Guid bucketId, Guid materialId, decimal slagWeight, DateTime startDate)
    {
        if (State != StateFieldType.Initial && State != StateFieldType.BucketRemoved)
            throw new DomainException(SlagFieldStateErrors.PlaceOccupied(Id)); // Валидация состояния

        var @event = new BucketPlacedEvent(
            EventId:Guid.NewGuid(), 
            AggregateId:Id, 
            EventType:"PlaceBucket", 
            Timestamp:DateTime.UtcNow, 
            BucketId:bucketId,
            MaterialId:materialId, 
            SlagWeight:slagWeight,
            ClientStartDate:startDate
        );
        
        AddEvent(@event); // Добавляем событие для сохранения
    }

    //Опустошить ковш
    public void EmptyBucket(DateTime emptyDate)
    {
        var @event = new BucketEmptiedEvent(
            EventId:Guid.NewGuid(), 
            AggregateId:Id, 
            EventType:"EmptyBucket", 
            Timestamp:DateTime.UtcNow,
            BucketEmptiedTime:emptyDate);
        
        AddEvent(@event);
    }
    
    // Метод для снятия ковша, возможно только если состояние BucketPlaced
    public void RemoveBucket()
    {
        var @event = new BucketRemovedEvent(
            EventId:Guid.NewGuid(), 
            AggregateId:Id, 
            EventType:"RemoveBucket", 
            Timestamp:DateTime.UtcNow);
        
        AddEvent(@event);
    }

    public void SetInvalid(string description)
    {
        var @event = new InvalidEvent(
            EventId: Guid.NewGuid(),
            AggregateId: Id,
            EventType: "Invalid",
            Timestamp: DateTime.UtcNow,
            Description: description);
        
        AddEvent(@event);
    }
    
    protected override void ApplyEvent(IDomainEvent @event)
    {
        switch (@event)
        {
            case BucketPlacedEvent placed:
                BucketId = placed.BucketId;
                MaterialId = placed.MaterialId;
                SlagWeight = placed.SlagWeight;
                StartDate = placed.ClientStartDate;
                State = StateFieldType.BucketPlaced;
                break;
            case BucketEmptiedEvent emptied:
                State = StateFieldType.BucketEmptied;
                EndDate = emptied.BucketEmptiedTime;
                break;
            case BucketRemovedEvent:
                BucketId = null;
                MaterialId = null;
                SlagWeight = 0;
                State = StateFieldType.BucketRemoved;
                break;
            case InvalidEvent invalid:
                BucketId = null;
                MaterialId = null;
                SlagWeight = 0;
                State = StateFieldType.Invalid;
                Description = invalid.Description;
                break;
        }
    }
}