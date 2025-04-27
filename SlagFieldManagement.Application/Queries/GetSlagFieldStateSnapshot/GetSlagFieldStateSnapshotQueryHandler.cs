using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Application.DTO;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;
using SlagFieldManagement.Domain.Events.SlagFieldPlace;
using SlagFieldManagement.Domain.Events.SlagFieldState;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Queries.GetSlagFieldStateSnapshot;

public class GetSlagFieldStateSnapshotQueryHandler:IQueryHandler<GetSlagFieldSnapshotQuery, List<SlagFieldStateResponse>>
{
    private readonly ISlagFieldPlaceRepository _placeRepository;
    private readonly IPlaceEventStore _placeEventStore;
    private readonly IStateEventStore _stateEventStore;

    public GetSlagFieldStateSnapshotQueryHandler(
        ISlagFieldPlaceRepository placeRepository,
        IPlaceEventStore placeEventStore,
        IStateEventStore stateEventStore)
    {
        _placeRepository = placeRepository;
        _placeEventStore = placeEventStore;
        _stateEventStore = stateEventStore;
    }
    public async Task<Result<List<SlagFieldStateResponse>>> Handle(GetSlagFieldSnapshotQuery request, CancellationToken cancellationToken)
    {
        // 1. Получаем все места
        var places = await _placeRepository.GetAllAsync(cancellationToken);
        var placeIds = places.Select(p => p.Id).ToList();
        
        // 2. Загружаем события для всех мест одним запросом
        var allPlaceEvents = await _placeEventStore.GetEventsBeforeForPlacesAsync(placeIds, request.SnapshotTime, cancellationToken);
        var allStateEvents = await _stateEventStore.GetEventsBeforeForPlacesAsync(placeIds, request.SnapshotTime, cancellationToken);
        
        // 3. Группируем события по placeId для быстрого доступа
        var placeEventsGrouped = allPlaceEvents
            .GroupBy(e => e.AggregateId)
            .ToDictionary(g => g.Key, g => g.OrderBy(e => e.Timestamp).ToList());

        var stateEventsGrouped = allStateEvents
            .GroupBy(e => e.AggregateId)
            .ToDictionary(g => g.Key, g => g.OrderBy(e => e.Timestamp).ToList());
        
        // 4. Формируем ответ для каждого места
        var responses = places.Select(place =>
        {
            var placeEventsForPlace = placeEventsGrouped.TryGetValue(place.Id, out var pe) ? pe : new List<IDomainEvent>();
            bool isEnable = DetermineIsEnable(placeEventsForPlace);

            string state;
            Guid? bucketId = null;
            Guid? materialId = null;
            decimal? slagWeight = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            string? description = null;

            if (isEnable)
            {
                var stateEventsForPlace = stateEventsGrouped.TryGetValue(place.Id, out var se) ? se : new List<IDomainEvent>();
                (state, bucketId, materialId, slagWeight, startDate, endDate, description) = RebuildState(stateEventsForPlace);
            }
            else
            {
                state = "NotInUse";
            }

            return new SlagFieldStateResponse()
            {
                PlaceId = place.Id,
                Row = place.Row,
                Number = place.Number,
                IsEnable = isEnable,
                State = state,
                BucketId = bucketId,
                MaterialId = materialId,
                SlagWeight = slagWeight,
                StartDate = startDate,
                EndDate = endDate,
                Description = description
            };
        }).ToList();

        return Result.Success(responses);
    }
    
    private bool DetermineIsEnable(List<IDomainEvent> placeEvents)
    {
        bool isEnable = false;
        foreach (var @event in placeEvents)
        {
            if (@event is WentInUseEvent) isEnable = true;
            else if (@event is WentOutOfUseEvent) isEnable = false;
        }
        return isEnable;
    }
    
    private (string state, Guid? bucketId, Guid? materialId, decimal? slagWeight, DateTime? startDate, DateTime? endDate, string? description) RebuildState(IEnumerable<IDomainEvent> stateEvents)
    {
        string state = "NotInUse";
        Guid? bucketId = null;
        Guid? materialId = null;
        decimal? slagWeight = null;
        DateTime? startDate = null;
        DateTime? endDate = null;
        string? description = null;

        foreach (var @event in stateEvents)
        {
            switch (@event)
            {
                case BucketPlacedEvent placed:
                    state = "BucketPlaced";
                    bucketId = placed.BucketId;
                    materialId = placed.MaterialId;
                    slagWeight = placed.SlagWeight;
                    startDate = placed.ClientStartDate;
                    endDate = null;
                    description = null;
                    break;
                case BucketEmptiedEvent emptied:
                    state = "BucketEmptied";
                    endDate = emptied.BucketEmptiedTime;
                    break;
                case BucketRemovedEvent:
                    state = "BucketRemoved";
                    bucketId = null;
                    materialId = null;
                    slagWeight = 0;
                    break;
                case InvalidEvent invalid:
                    state = "Invalid";
                    description = invalid.Description;
                    break;
            }
        }

        return (state, bucketId, materialId, slagWeight, startDate, endDate, description);
    }
}