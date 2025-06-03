using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Events.SlagFieldState;
using SlagFieldManagement.Domain.Interfaces;
using SlagFieldManagement.Infrastructure.EventStores;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldStateEventStore : IStateEventStore
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
        /*var state = await _dbContext.SlagFieldStates.FindAsync(new object[]{ stateId });
        if (state == null)
            throw new InvalidOperationException($"State with id {stateId} not found");
        var placeId = state.PlaceId;*/

        var eventEntity = new SlagFieldStateEvent
        {
            EventId = @event.EventId,
            AggregateId = stateId, // теперь placeId
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
        // 1) LEFT JOIN stateEvents → SlagFieldStates, чтобы вычислить настоящий placeId
        var rows = await (
            from e in _dbContext.SlagFieldStateEvents
            join st in _dbContext.SlagFieldStates
                on e.AggregateId equals st.Id into stg
            from s in stg.DefaultIfEmpty()
            let placeId = s != null ? s.PlaceId : e.AggregateId
            where placeIds.Contains(placeId)
                  && e.Timestamp <= snapshotTime
            orderby placeId, e.Timestamp
            select new
            {
                e.EventId,
                e.EventType,
                e.EventData,
                e.Timestamp,
                e.Metadata,
                e.Version,
                PlaceId = placeId
            }
        ).ToListAsync(cancellationToken);

        // 2) Десериализуем каждую строку в доменное событие, выставляя AggregateId = placeId
        var result = new List<IDomainEvent>(rows.Count);
        foreach (var row in rows)
        {
            IDomainEvent de = row.EventType switch
            {
                "PlaceBucket" => new BucketPlacedEvent(
                    EventId: row.EventId,
                    AggregateId: row.PlaceId,
                    EventType: row.EventType,
                    Timestamp: row.Timestamp,
                    BucketId: JsonDocument.Parse(row.EventData)
                        .RootElement.GetProperty("BucketId").GetGuid(),
                    MaterialId: JsonDocument.Parse(row.EventData)
                        .RootElement.GetProperty("MaterialId").GetGuid(),
                    SlagWeight: JsonDocument.Parse(row.EventData)
                        .RootElement.GetProperty("SlagWeight").GetDecimal(),
                    ClientStartDate: JsonDocument.Parse(row.EventData)
                        .RootElement.GetProperty("ClientStartDate").GetDateTime()
                ),

                "EmptyBucket" => new BucketEmptiedEvent(
                    EventId: row.EventId,
                    AggregateId: row.PlaceId,
                    EventType: row.EventType,
                    Timestamp: row.Timestamp,
                    BucketEmptiedTime: JsonDocument.Parse(row.EventData)
                        .RootElement.GetProperty("BucketEmptiedTime").GetDateTime()
                ),

                "RemoveBucket" => new BucketRemovedEvent(
                    EventId: row.EventId,
                    AggregateId: row.PlaceId,
                    EventType: row.EventType,
                    Timestamp: row.Timestamp
                ),

                "Invalid" => new InvalidEvent(
                    EventId: row.EventId,
                    AggregateId: row.PlaceId,
                    EventType: row.EventType,
                    Timestamp: row.Timestamp,
                    Description: JsonDocument.Parse(row.EventData)
                        .RootElement.GetProperty("Description").GetString()!
                ),

                _ => throw new InvalidOperationException($"Unknown event type: {row.EventType}")
            };

            result.Add(de);
        }

        return result;
    }
}