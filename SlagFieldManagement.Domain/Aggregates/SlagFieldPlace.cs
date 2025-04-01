using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Domain.Aggregates;

public sealed class SlagFieldPlace:AggregateRoot
{
    public string Row { get; private set; }
    public int Number { get; private set; }
    public bool IsEnable { get; private set; }
    public bool IsDelete { get; private set; }
    
    // Состояние (проекция)
    //public SlagFieldState CurrentState { get; private set; }
    private SlagFieldPlace(Guid id) : base(id)
    {
        IsEnable = false;
        IsDelete = false;
    }
    
    // Фабричный метод
    public static SlagFieldPlace Create(string row, int number)
    {
        if (string.IsNullOrWhiteSpace(row))
            throw new ArgumentException("Строка не может быть пустой.");
        if (number <= 0)
            throw new ArgumentException("Номер должен быть положительным.");
        var place = new SlagFieldPlace(Guid.NewGuid())
        {
            Row = row,
            Number = number
        };
        //place.AddDomainEvent(new PlaceCreatedEvent(place.Id, row, number));
        return place;
    }
    
    // Активация места
    public void Enable()
    {
        //TODO Domain exception в разработке
        /*if (IsEnabled) 
            throw new DomainException("Место уже активно.");*/

        var @event = new PlaceWentInUseEvent { AggregateId = Id };
        AddDomainEvent(@event);
        IsEnable = true;
    }
    
    // Деактивация места
    public void Disable()
    {
        /*if (!IsEnabled)
            throw new DomainException("Место уже отключено.");*/

        // Если есть активный ковш - помечаем как Invalid
        /*if (CurrentState.State != StateFieldType.BucketRemoved)
        {
            CurrentState = SlagFieldState.Invalid(CurrentState, "Принудительная деактивация");
            AddDomainEvent(new StateMarkedInvalidEvent(Id, "Принудительная деактивация"));
        }*/

        var @event = new PlaceWentOutOfUseEvent { AggregateId = Id };
        AddDomainEvent(@event);
        IsEnable = false;
    }
    
    // Восстановление из событий
    public override void LoadFromHistory(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            switch (@event.EventType)
            {
                case "PlaceWentInUse":
                    IsEnable = true;
                    break;
                case "PlaceWentOutOfUse":
                    IsEnable = false;
                    break;
                default:
                    throw new NotSupportedException("Неподдерживаемый тип события");
            }
        }
    }
}