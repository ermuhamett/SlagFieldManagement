using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;

namespace SlagFieldManagement.Application.Queries.GetAllPlaces;

public class GetAllPlacesQueryHandler:IQueryHandler<GetAllPlacesQuery, List<SlagFieldPlace>>
{
    private readonly ISlagFieldPlaceRepository _repository;

    public GetAllPlacesQueryHandler(ISlagFieldPlaceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<SlagFieldPlace>>> Handle(
        GetAllPlacesQuery request, 
        CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}