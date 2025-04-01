using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Interfaces;
using SlagFieldManagement.Domain.ValueObjects;

namespace SlagFieldManagement.Domain.Aggregates;

public class SlagFieldState:AggregateRoot
{
    public Guid PlaceId { get; private set; }
    public Guid? BucketId { get; private set; }
    public Guid? MaterialId { get; private set; }
    public SlagWeight SlagWeight { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Description { get; private set; }
    public StateFieldType State { get; private set; }
    public bool IsDelete { get; private set; }
    
    private SlagFieldState(Guid id) : base(id)
    {
        State = StateFieldType.Initial;
    }
    
    // Фабричный метод для создания нового состояния для места
    public static SlagFieldState Create(Guid id, Guid placeId)
    {
        var state = new SlagFieldState(id);
        state.PlaceId = placeId;
        return state;
    }
    
    // Метод для установки ковша на место, возможно только если состояние Initial или BucketRemoved
    public void PlaceBucket(Guid bucketId, Guid materialId, decimal slagWeight, DateTime startDate)
    {
        /*if (State != StateFieldType.Initial && State != StateFieldType.BucketRemoved)
            throw new DomainException("Невозможно установить ковш: место занято или недоступно."); // Валидация состояния*/
        
        var @event = new BucketPlacedEvent
        {
            AggregateId = Id,
            PlaceId = PlaceId,
            BucketId = bucketId,
            MaterialId = materialId,
            SlagWeight = slagWeight,
            StartDate = startDate
        };
        AddDomainEvent(@event); // Добавляем событие для сохранения
        BucketId = bucketId; // Обновляем состояние
        MaterialId = materialId;
        SlagWeight = SlagWeight.FromKilograms(slagWeight);
        StartDate = startDate;
        State = StateFieldType.BucketPlaced; // Обновляем до нового состояния
    }
    
    // Метод для снятия ковша, возможно только если состояние BucketPlaced
    public void RemoveBucket(DateTime endDate)
    {
        /*if (State != SlagFieldStateType.BucketPlaced)
            throw new DomainException("Невозможно снять ковш: состояние не BucketPlaced.");*/

        var @event = new BucketRemovedEvent { AggregateId = Id, EndDate = endDate };
        AddDomainEvent(@event);

        BucketId = null;
        MaterialId = null;
        SlagWeight = SlagWeight.Zero;
        EndDate = endDate;
        State = StateFieldType.BucketRemoved; // Обновляем до нового состояния
    }

    public override void LoadFromHistory(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            switch (@event.EventType)
            {
                case "BucketPlaced":
                    var placedEvent = (BucketPlacedEvent)@event;
                    BucketId = placedEvent.BucketId;
                    MaterialId = placedEvent.MaterialId;
                    SlagWeight = placedEvent.SlagWeight;
                    StartDate = placedEvent.StartDate;
                    State = StateFieldType.BucketPlaced; // Применяем событие для установки состояния
                    break;
                case "BucketRemoved":
                    var removedEvent = (BucketRemovedEvent)@event;
                    BucketId = null;
                    MaterialId = null;
                    SlagWeight = null;
                    EndDate = removedEvent.EndDate;
                    State = StateFieldType.BucketRemoved; // Применяем событие для установки состояния
                    break;
                default:
                    throw new NotSupportedException("Неподдерживаемый тип события"); // Обрабатываем неизвестные события
            }
        }
    }
}