namespace SlagFieldManagement.Domain.Interfaces;

public interface IAggregateRoot
{
    Guid Id { get; }
    void LoadFromHistory(IEnumerable<IDomainEvent> events);
    IEnumerable<IDomainEvent> GetUncommittedEvents();
    void ClearUncommittedEvents();
}