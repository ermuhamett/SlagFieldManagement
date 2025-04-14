namespace SlagFieldManagement.Domain.Interfaces;

public interface IPlaceEventStore
{
    Task SaveEventAsync(IDomainEvent @event, Guid placeId, int expectedVersion);
    Task<IEnumerable<IDomainEvent>> GetEventsForPlaceAsync(Guid placeId);
    Task<List<IDomainEvent>> GetEventsBeforeAsync(Guid placeId, DateTime time, CancellationToken cancellationToken = default);
    
    // Новый метод для пакетной загрузки
    Task<List<IDomainEvent>> GetEventsBeforeForPlacesAsync(
        List<Guid> placeIds, 
        DateTime snapshotTime, 
        CancellationToken cancellationToken = default
    );
}