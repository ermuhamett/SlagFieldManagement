using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Application.DTO;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;
using SlagFieldManagement.Domain.Events.SlagFieldPlace;
using SlagFieldManagement.Domain.Events.SlagFieldState;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Queries.GetSlagFieldHistory;

internal sealed class GetSlagFieldHistoryQueryHandler : 
    IQueryHandler<GetSlagFieldHistoryQuery, List<SlagFieldEventHistoryResponse>>
{
    private readonly ISlagFieldPlaceRepository _placeRepository;
    private readonly IPlaceEventStore _placeEventStore;
    private readonly IStateEventStore _stateEventStore;

    public GetSlagFieldHistoryQueryHandler(
        ISlagFieldPlaceRepository placeRepository,
        IPlaceEventStore placeEventStore,
        IStateEventStore stateEventStore)
    {
        _placeRepository = placeRepository;
        _placeEventStore = placeEventStore;
        _stateEventStore = stateEventStore;
    }

    public async Task<Result<List<SlagFieldEventHistoryResponse>>> Handle(
        GetSlagFieldHistoryQuery request,
        CancellationToken ct)
    {
        // Получаем список всех placeIds (чтобы фильтровать события только по ним)
        var places = await _placeRepository.GetAllAsync(ct);
        var placeIds = places.Select(p => p.Id).ToList();

        var snapshotEnd = request.Timestamp.TimeOfDay == TimeSpan.Zero
            ? request.Timestamp.AddDays(1).AddTicks(-1) // конец дня
            : request.Timestamp;
        
        // Загружаем ВСЕ события мест до timestamp
        var placeEvts = await _placeEventStore
            .GetEventsBeforeForPlacesAsync(placeIds, snapshotEnd, ct);
        Console.WriteLine($"Loaded {placeEvts.Count} place events");
        
        // Загружаем ВСЕ события состояний до timestamp
        var stateEvts = await _stateEventStore
            .GetEventsBeforeForPlacesAsync(placeIds, snapshotEnd, ct);
        Console.WriteLine($"Loaded {stateEvts.Count} state events");
        
        // Сливаем и сортируем
        var allEvts = placeEvts
            .Cast<IDomainEvent>()
            .Concat(stateEvts)
            .OrderBy(e => e.Timestamp);

        // Маппим каждое событие в DTO
        var history = new List<SlagFieldEventHistoryResponse>();
        foreach (var e in allEvts)
        {
            var dto = new SlagFieldEventHistoryResponse
            {
                EventId = e.EventId,
                PlaceId = e.AggregateId,
                EventType = e.EventType,
                Timestamp = e.Timestamp
            };

            // если это state‑событие, проставляем дополнительные поля
            switch (e)
            {
                case BucketPlacedEvent bp:
                    dto = dto with
                    {
                        EventType = bp.EventType,
                        BucketId = bp.BucketId,
                        MaterialId = bp.MaterialId,
                        SlagWeight = bp.SlagWeight,
                        ClientStartDate = bp.ClientStartDate
                    };
                    break;

                case BucketEmptiedEvent be:
                    dto = dto with
                    {
                        EventType = be.EventType,
                        BucketEmptiedTime = be.BucketEmptiedTime
                    };
                    break;

                case InvalidEvent inv:
                    dto = dto with
                    {
                        EventType = inv.EventType,
                        Timestamp = inv.Timestamp,
                        Description = inv.Description
                    };
                    break;

                case BucketRemovedEvent rem:
                    dto = dto with
                    {
                        EventType = rem.EventType,
                        Timestamp = rem.Timestamp
                    };
                    break;

                case WentInUseEvent up:
                    dto = dto with
                    {
                        EventType = up.EventType,
                        Timestamp = up.Timestamp
                    };
                    break;

                case WentOutOfUseEvent op:
                    dto = dto with
                    {
                        EventType = op.EventType,
                        Timestamp = op.Timestamp
                    };
                    break;
            }

            history.Add(dto);
        }

        return Result.Success(history);
    }
}