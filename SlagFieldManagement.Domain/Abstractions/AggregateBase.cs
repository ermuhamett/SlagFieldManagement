using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Domain.Abstractions;

public abstract class AggregateBase : Entity
{
    private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
    
    public IReadOnlyList<IDomainEvent> Events => _events.AsReadOnly();

    protected AggregateBase(Guid id) : base(id) {}
    
    // Метод для добавления события
    protected void AddEvent(IDomainEvent @event)
    {
        _events.Add(@event);
        ApplyEvent(@event); // Применяем событие сразу для обновления состояния
    }

    // Абстрактный метод для применения события
    protected abstract void ApplyEvent(IDomainEvent @event);

    //public abstract void LoadFromHistory(IEnumerable<IDomainEvent> events);
}