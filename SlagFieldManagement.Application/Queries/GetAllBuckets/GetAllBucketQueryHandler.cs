using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Queries.GetAllBuckets;

public sealed class GetAllBucketsQueryHandler:IQueryHandler<GetAllBucketsQuery, List<Bucket>>
{
    private readonly IBucketRepository _repository;

    public GetAllBucketsQueryHandler(IBucketRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<List<Bucket>>> Handle(GetAllBucketsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllBuckets(cancellationToken);
    }
}