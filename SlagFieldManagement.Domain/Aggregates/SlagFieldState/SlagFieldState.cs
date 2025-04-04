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
    
    private SlagFieldState(Guid id, Guid placeId, DateTime startDate) : base(id)
    {
        PlaceId = placeId;
        StartDate = startDate;
        State = StateFieldType.Initial;
    }
    
    // Фабричный метод для создания нового состояния для места
    public static SlagFieldState Create(Guid id, Guid placeId,DateTime startDate)
    {
        return new SlagFieldState(id, placeId, startDate);
    }
    
    // Метод для установки ковша на место, возможно только если состояние Initial или BucketRemoved
    public void PlaceBucket(Guid bucketId, Guid materialId, decimal slagWeight, DateTime startDate)
    {
        if (State != StateFieldType.Initial && State != StateFieldType.BucketRemoved)
            throw new DomainException(SlagFieldStateErrors.PlaceOccupied(Id)); // Валидация состояния

        var @event = new BucketPlacedEvent(
            Guid.NewGuid(), 
            Id, 
            StateFieldType.BucketPlaced.ToString(), 
            DateTime.UtcNow, 
            bucketId,
            materialId, 
            slagWeight);
        AddEvent(@event); // Добавляем событие для сохранения
    }

    //Опустошить ковш
    public void EmptyBucket()
    {
        if (State != StateFieldType.BucketPlaced)
            throw new DomainException(SlagFieldStateErrors.EmptyingNotAllowed);
        var @event = new BucketEmptiedEvent(
            Guid.NewGuid(), 
            Id, 
            StateFieldType.BucketEmptied.ToString(), 
            DateTime.UtcNow);
        AddEvent(@event);
    }
    
    // Метод для снятия ковша, возможно только если состояние BucketPlaced
    public void RemoveBucket(DateTime endDate)
    {
        if (State != StateFieldType.BucketEmptied)
            throw new DomainException(SlagFieldStateErrors.RemovalNotAllowed);

        var @event = new BucketRemovedEvent(
            Guid.NewGuid(), 
            Id, 
            StateFieldType.BucketRemoved.ToString(), 
            DateTime.UtcNow);
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
                State = StateFieldType.BucketPlaced;
                break;
            case BucketEmptiedEvent emptied:
                State = StateFieldType.BucketEmptied;
                EndDate = emptied.Timestamp;
                break;
            case BucketRemovedEvent removed:
                BucketId = null;
                MaterialId = null;
                SlagWeight = 0;
                State = StateFieldType.BucketRemoved;
                EndDate = removed.Timestamp;
                break;
            case InvalidEvent invalid:
                State = StateFieldType.Invalid;
                Description = invalid.Description;
                break;
        }
    }
}