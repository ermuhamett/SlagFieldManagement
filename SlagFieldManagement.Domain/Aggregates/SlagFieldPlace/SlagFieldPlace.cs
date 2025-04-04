using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Events.SlagFieldPlace;
using SlagFieldManagement.Domain.Exceptions;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;

public sealed class SlagFieldPlace:AggregateBase
{
    public string Row { get; private set; }
    public int Number { get; private set; }
    public bool IsEnable { get; private set; }
    public bool IsDelete { get; private set; }
    
    // Состояние (проекция)
    //public SlagFieldState CurrentState { get; private set; }
    private SlagFieldPlace(Guid id, string row, int number) : base(id)
    {
        Row = row;
        Number = number;
        IsEnable = false;
        IsDelete = false;
    }
    
    // Фабричный метод
    public static SlagFieldPlace Create(string row, int number)
    {
        if (string.IsNullOrWhiteSpace(row) || !row.All(char.IsLetterOrDigit))
            throw new DomainException(SlagFieldPlaceErrors.InvalidRowFormat);

        if (number <= 0)
            throw new DomainException(SlagFieldPlaceErrors.InvalidNumber);

        return new SlagFieldPlace(Guid.NewGuid(), row, number);
    }
    
    // Активация места
    public void Enable()
    {
        if (IsEnable)
            throw new DomainException(SlagFieldPlaceErrors.AlreadyEnabled(Id));
        
        var @event = new WentInUseEvent(Guid.NewGuid(), 
            Id, 
            StatePlaceType.PlaceWentInUse.ToString(),
            DateTime.UtcNow);
        AddEvent(@event);
    }
    
    // Деактивация места
    public void Disable()
    {
        if (!IsEnable)
            throw new DomainException(SlagFieldPlaceErrors.AlreadyDisabled(Id));

        // Если есть активные операции
        /*if (HasActiveOperations())
            throw new DomainException(SlagFieldPlaceErrors.HasActiveOperations(Id));*/

        var @event = new WentOutOfUseEvent(Guid.NewGuid(),
            Id,
            StatePlaceType.PlaceWentOutOfUse.ToString(),
            DateTime.UtcNow);
        
        AddEvent(@event);
    }
    

    protected override void ApplyEvent(IDomainEvent @event)
    {
        switch (@event)
        {
            case WentInUseEvent:
                IsEnable = true;
                break;
            case WentOutOfUseEvent:
                IsEnable = false;
                break;
        }
    }
}