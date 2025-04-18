using SlagFieldManagement.Domain.Interfaces;
using SlagFieldManagement.Infrastructure.EventStores;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldPlaceEventStore:IPlaceEventStore
{
    private readonly ApplicationDbContext _dbContext;

    public SlagFieldPlaceEventStore(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task SaveEventAsync(IDomainEvent @event, Guid placeId, int expectedVersion)
    {
        var eventEntity = new SlagFieldPlaceEvent
        {
            EventId = @event.EventId,
            AggregateId = placeId,
            EventType = @event.EventType,
            EventData = @event.EventData,
            Version = @event.Version,
            Timestamp = @event.Timestamp,
            Metadata = @event.Metadata
        };

        await _dbContext.AddAsync(eventEntity);
    }

    public Task<List<IDomainEvent>> GetEventsBeforeForPlacesAsync(List<Guid> placeIds, DateTime snapshotTime, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}