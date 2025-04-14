using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Events.SlagFieldPlace;
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
    public static Result<SlagFieldPlace> Create(string row, int number)
    {
        if (string.IsNullOrWhiteSpace(row) || !row.All(char.IsLetterOrDigit))
            return Result.Failure<SlagFieldPlace>(SlagFieldPlaceErrors.InvalidRowFormat);

        if (number <= 0)
            return Result.Failure<SlagFieldPlace>(SlagFieldPlaceErrors.InvalidNumber);

        return Result.Success(new SlagFieldPlace(Guid.NewGuid(), row, number));
    }
    
    // Активация места
    public Result Enable()
    {
        if (IsEnable)
            return Result.Failure(SlagFieldPlaceErrors.AlreadyEnabled(Id));
        
        var @event = new WentInUseEvent(
            EventId:Guid.NewGuid(), 
            AggregateId:Id, 
            EventType:"WentInUse",
            Timestamp:DateTime.UtcNow);
        
        AddEvent(@event);
        return Result.Success();
    }
    
    // Деактивация места
    public Result Disable()
    {
        if (!IsEnable)
            return Result.Failure(SlagFieldPlaceErrors.AlreadyDisabled(Id));
        
        var @event = new WentOutOfUseEvent(
            EventId:Guid.NewGuid(),
            AggregateId:Id,
            EventType:"OutOfUse",
            Timestamp:DateTime.UtcNow);
        
        AddEvent(@event);
        return Result.Success();
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