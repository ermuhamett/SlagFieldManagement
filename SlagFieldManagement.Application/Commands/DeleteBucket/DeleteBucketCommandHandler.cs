using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Exceptions;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Commands.DeleteBucket;

public sealed class DeleteBucketCommandHandler:ICommandHandler<DeleteBucketCommand>
{
    private readonly IBucketRepository _repository;

    public DeleteBucketCommandHandler(IBucketRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result> Handle(
        DeleteBucketCommand request, 
        CancellationToken cancellationToken)
    {
        var bucket = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if(bucket is null || bucket.IsDelete)
            return Result.Failure(SlagFieldErrors.BucketNotFound(request.Id));

        await _repository.DeleteAsync(bucket, cancellationToken);
        return Result.Success();
    }
}