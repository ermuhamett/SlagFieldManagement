namespace SlagFieldManagement.Domain.Interfaces;

public interface IPlaceEventStore
{
    Task SaveEventAsync(IDomainEvent @event, Guid placeId, int expectedVersion);
    // Новый метод для пакетной загрузки
    Task<List<IDomainEvent>> GetEventsBeforeForPlacesAsync(
        List<Guid> placeIds, 
        DateTime snapshotTime, 
        CancellationToken cancellationToken = default
    );
}