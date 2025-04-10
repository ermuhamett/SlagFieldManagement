namespace SlagFieldManagement.Domain.Interfaces;

public interface IStateEventStore
{
    Task SaveEventAsync(IDomainEvent @event, Guid stateId, int expectedVersion);
    Task<IEnumerable<IDomainEvent>> GetEventsForStateAsync(Guid stateId);
}