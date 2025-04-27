using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Events.SlagFieldPlace;
using SlagFieldManagement.Domain.Interfaces;
using SlagFieldManagement.Infrastructure.EventStores;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldPlaceEventStore:IPlaceEventStore
{
    private readonly ApplicationDbContext _dbContext;

    // Словарь для десериализации событий
    private static readonly Dictionary<string, Func<string, IDomainEvent>> EventDeserializers = new()
    {
        ["WentInUse"] = json => JsonSerializer.Deserialize<WentInUseEvent>(json)!,
        ["OutOfUse"] = json => JsonSerializer.Deserialize<WentOutOfUseEvent>(json)!,
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
            var deserializedEvent = deserializer(storageEvent.EventData);
            if (deserializedEvent == null)
                throw new InvalidOperationException($"Failed to deserialize event: {storageEvent.EventType}");
            return deserializedEvent;
        }
        throw new InvalidOperationException($"Unknown event type: {storageEvent.EventType}");
    }
}