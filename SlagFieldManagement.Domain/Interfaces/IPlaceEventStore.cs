namespace SlagFieldManagement.Domain.Interfaces;

public interface IPlaceEventStore
{
    Task SaveEventAsync(IDomainEvent @event, Guid placeId, int expectedVersion);
    Task<IEnumerable<IDomainEvent>> GetEventsForPlaceAsync(Guid placeId);
}