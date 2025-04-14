namespace SlagFieldManagement.Domain.Interfaces;

public interface IStateEventStore
{
    Task SaveEventAsync(IDomainEvent @event, Guid stateId, int expectedVersion);
    Task<IEnumerable<IDomainEvent>> GetEventsBeforeAsync(Guid stateId, DateTime time, CancellationToken cancellationToken=default);
    
    // Новый метод для пакетной загрузки
    Task<List<IDomainEvent>> GetEventsBeforeForPlacesAsync(
        List<Guid> placeIds, 
        DateTime snapshotTime, 
        CancellationToken cancellationToken = default
    );
}