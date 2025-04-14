using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Application.DTO;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Queries.GetSlagFieldState;

internal sealed class GetSlagFieldStateQueryHandler:IQueryHandler<GetSlagFieldStateQuery, List<SlagFieldStateResponse>>
{
    private readonly ISlagFieldQueryRepository _slagFieldQueryRepository;

    public GetSlagFieldStateQueryHandler(ISlagFieldQueryRepository slagFieldQueryRepository)
    {
        _slagFieldQueryRepository = slagFieldQueryRepository;
    }

    public async Task<Result<List<SlagFieldStateResponse>>> Handle(
        GetSlagFieldStateQuery request, 
        CancellationToken ct)
    {
        var projections = await _slagFieldQueryRepository.GetCurrentStatesAsync(ct);
        var response = projections.Select(p => new SlagFieldStateResponse
        {
            PlaceId = p.PlaceId,
            Row = p.Row,
            Number = p.Number,
            IsEnable = p.IsEnable,
            StateId = p.StateId,
            State = p.State,
            BucketId = p.BucketId,
            MaterialId = p.MaterialId,
            SlagWeight = p.SlagWeight,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            Description = p.Description
        }).ToList();

        return Result.Success(response);
    }
}