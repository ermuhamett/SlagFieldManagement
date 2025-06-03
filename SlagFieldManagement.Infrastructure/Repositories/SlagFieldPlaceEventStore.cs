using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Events.SlagFieldPlace;
using SlagFieldManagement.Domain.Interfaces;
using SlagFieldManagement.Infrastructure.EventStores;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldPlaceEventStore:IPlaceEventStore
{
    private readonly ApplicationDbContext _dbContext;

    // Вместо Func<string,IDomainEvent> теперь принимаем всю сущность
    private static readonly Dictionary<string, Func<SlagFieldPlaceEvent, IDomainEvent>> EventDeserializers
        = new()
        {
            ["WentInUse"]  = se => new WentInUseEvent(
                EventId: se.EventId,
                AggregateId: se.AggregateId,
                EventType: se.EventType,
                Timestamp: se.Timestamp),
            ["OutOfUse"]   = se => new WentOutOfUseEvent(
                EventId: se.EventId,
                AggregateId: se.AggregateId,
                EventType: se.EventType,
                Timestamp: se.Timestamp),
        };
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

        await _dbContext.SlagFieldPlaceEvents.AddAsync(eventEntity);
    }

    public async Task<List<IDomainEvent>> GetEventsBeforeForPlacesAsync(
        List<Guid> placeIds, 
        DateTime snapshotTime, 
        CancellationToken ct)
    {
        var storageEvents = await _dbContext.SlagFieldPlaceEvents
            .Where(e => placeIds.Contains(e.AggregateId) && e.Timestamp <= snapshotTime)
            .OrderBy(e => e.AggregateId)
            .ThenBy(e => e.Timestamp)
            .ToListAsync(ct);

        var domainEvents = storageEvents.Select(DeserializeEvent).ToList();
        return domainEvents;
    }
    
    private IDomainEvent DeserializeEvent(SlagFieldPlaceEvent storageEvent)
    {
        if (EventDeserializers.TryGetValue(storageEvent.EventType, out var deserializer))
        {
            return deserializer(storageEvent);
        }
        throw new InvalidOperationException($"Unknown event type: {storageEvent.EventType}");
    }
}