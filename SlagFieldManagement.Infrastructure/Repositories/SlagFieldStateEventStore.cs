using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Events.SlagFieldState;
using SlagFieldManagement.Domain.Interfaces;
using SlagFieldManagement.Infrastructure.EventStores;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldStateEventStore:IStateEventStore
{
    private readonly ApplicationDbContext _dbContext;

    // Словарь для десериализации событий
    private static readonly Dictionary<string, Func<string, IDomainEvent>> EventDeserializers = new()
    {
        ["PlaceBucket"] = json => JsonSerializer.Deserialize<BucketPlacedEvent>(json)!,
        ["EmptyBucket"] = json => JsonSerializer.Deserialize<BucketEmptiedEvent>(json)!,
        ["RemoveBucket"] = json => JsonSerializer.Deserialize<BucketRemovedEvent>(json)!,
        ["Invalid"] = json => JsonSerializer.Deserialize<InvalidEvent>(json)!
    };
    
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
        await _dbContext.SlagFieldStateEvents.AddAsync(eventEntity);
    }

    public async Task<List<IDomainEvent>> GetEventsBeforeForPlacesAsync(
        List<Guid> placeIds, 
        DateTime snapshotTime, 
        CancellationToken cancellationToken = default)
    {
        var storageEvents = await _dbContext.SlagFieldStateEvents
            .Where(e => placeIds.Contains(e.AggregateId) && e.Timestamp <= snapshotTime)
            .OrderBy(e => e.AggregateId)
            .ThenBy(e => e.Timestamp)
            .ToListAsync(cancellationToken);

        var domainEvents = storageEvents.Select(DeserializeEvent).ToList();
        return domainEvents;
    }
    
    private IDomainEvent DeserializeEvent(SlagFieldStateEvent storageEvent)
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