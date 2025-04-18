using SlagFieldManagement.Domain.Interfaces;
using SlagFieldManagement.Infrastructure.EventStores;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldStateEventStore:IStateEventStore
{
    private readonly ApplicationDbContext _dbContext;

    public SlagFieldStateEventStore(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task SaveEventAsync(
        IDomainEvent @event, 
        Guid stateId, 
        int expectedVersion)
    {
        var eventEntity = new SlagFieldStateEvent
        {
            EventId = @event.EventId,
            AggregateId = @event.AggregateId,
            EventType = @event.EventType,
            EventData = @event.EventData,
            Version = @event.Version,
            Timestamp = @event.Timestamp,
            Metadata = @event.Metadata
        };
        await _dbContext.AddAsync(eventEntity);
    }

    public Task<List<IDomainEvent>> GetEventsBeforeForPlacesAsync(
        List<Guid> placeIds, 
        DateTime snapshotTime, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}