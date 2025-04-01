using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Domain.Abstractions;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _uncommittedEvents = new();
    public IEnumerable<IDomainEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent @event) 
        => _uncommittedEvents.Add(@event);

    public void ClearUncommittedEvents() 
        => _uncommittedEvents.Clear();

    public abstract void LoadFromHistory(IEnumerable<IDomainEvent> events);
}