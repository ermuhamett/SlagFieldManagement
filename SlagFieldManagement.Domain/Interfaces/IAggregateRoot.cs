namespace SlagFieldManagement.Domain.Interfaces;

public interface IAggregateRoot
{
    int Id { get; }
    void LoadFromHistory(IEnumerable<IDomainEvent> events);
    IEnumerable<IDomainEvent> GetUncommittedEvents();
    void ClearUncommittedEvents();
}