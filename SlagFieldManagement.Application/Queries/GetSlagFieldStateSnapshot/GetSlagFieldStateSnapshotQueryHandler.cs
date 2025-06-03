using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Application.DTO;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;
using SlagFieldManagement.Domain.Events.SlagFieldPlace;
using SlagFieldManagement.Domain.Events.SlagFieldState;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Queries.GetSlagFieldStateSnapshot;

public class GetSlagFieldStateSnapshotQueryHandler
    : IQueryHandler<GetSlagFieldSnapshotQuery, List<SlagFieldStateResponse>>
{
    private readonly ISlagFieldPlaceRepository _placeRepo;
    private readonly IPlaceEventStore _placeStore;
    private readonly IStateEventStore _stateStore;

    public GetSlagFieldStateSnapshotQueryHandler(
        ISlagFieldPlaceRepository placeRepository,
        IPlaceEventStore placeEventStore,
        IStateEventStore stateEventStore)
    {
        _placeRepo = placeRepository;
        _placeStore = placeEventStore;
        _stateStore = stateEventStore;
    }

    public async Task<Result<List<SlagFieldStateResponse>>> Handle(
        GetSlagFieldSnapshotQuery request,
        CancellationToken ct)
    {
        var places = await _placeRepo.GetAllAsync(ct);
        var placeIds = places.Select(p => p.Id).ToList();

        var snapshotEnd = request.SnapshotTime.TimeOfDay == TimeSpan.Zero
            ? request.SnapshotTime.AddDays(1).AddTicks(-1) // конец дня
            : request.SnapshotTime;

        // Получаем события мест
        var placeEvts = await _placeStore
            .GetEventsBeforeForPlacesAsync(placeIds, snapshotEnd, ct);
        Console.WriteLine($"Loaded {placeEvts.Count} place events");

        // Получаем события состояний
        var stateEvts = await _stateStore
            .GetEventsBeforeForPlacesAsync(placeIds, snapshotEnd, ct);
        Console.WriteLine($"Loaded {stateEvts.Count} state events");

        // 3) сгруппировать сразу
        var placeLastEvt = placeEvts
            .GroupBy(e => e.AggregateId)
            .ToDictionary(
                g => g.Key,
                g => g.MaxBy(e => e.Timestamp)
            );
        var stateLastEvt = stateEvts
            .GroupBy(e => e.AggregateId)
            .ToDictionary(
                g => g.Key,
                g => g.MaxBy(e => e.Timestamp)
            );

        // 7) Собираем результат
        var result = new List<SlagFieldStateResponse>(places.Count);

        foreach (var place in places)
        {
            // Базовый «пустой» ответ
            var response = new SlagFieldStateResponse
            {
                PlaceId = place.Id,
                Row = place.Row,
                Number = place.Number,
                IsEnable = true,
                State = "NotInUse",
                BucketId = null,
                MaterialId = null,
                SlagWeight = null,
                StartDate = null,
                EndDate = null,
                Description = null
            };
            
            // Применяем последнее state‑событие поверх (оно имеет приоритет)
            if (stateLastEvt.TryGetValue(place.Id, out var stEvt) && stEvt != null)
            {
                response = stEvt switch
                {
                    BucketPlacedEvent p => response with
                    {
                        IsEnable = true,
                        State = "BucketPlaced",
                        BucketId = p.BucketId,
                        MaterialId = p.MaterialId,
                        SlagWeight = p.SlagWeight,
                        StartDate = p.ClientStartDate
                    },
                    BucketEmptiedEvent eb => response with
                    {
                        IsEnable = true,
                        State = "BucketEmptied",
                        EndDate = eb.BucketEmptiedTime
                    },
                    BucketRemovedEvent => response with
                    {
                        IsEnable = false,
                        State = "BucketRemoved",
                        BucketId = null,
                        MaterialId = null,
                        SlagWeight = 0
                    },
                    InvalidEvent inv => response with
                    {
                        State = "Invalid",
                        Description = inv.Description,
                        // при invalid ковш считается снятым
                        BucketId = null,
                        MaterialId = null,
                        SlagWeight = 0,
                        StartDate = null,
                        EndDate = null
                    },
                    _ => response
                };
            }

            // Применяем последнее place‑событие (активация/деактивация)
            if (placeLastEvt.TryGetValue(place.Id, out var plEvt) && plEvt != null)
            {
                response = plEvt switch
                {
                    WentInUseEvent => response with { IsEnable = true, State = "InUse" },
                    WentOutOfUseEvent => response with { IsEnable = false, State = "NotInUse" },
                    _ => response
                };
            }
            
            result.Add(response);
        }

        return Result.Success(result);
    }
}