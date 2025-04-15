using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Application.Commands.CreateBucket;

public sealed class CreateBucketCommandHandler:ICommandHandler<CreateBucketCommand, Bucket>
{
    private readonly IBucketRepository _bucketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBucketCommandHandler(
        IBucketRepository bucketRepository,
        IUnitOfWork unitOfWork)
    {
        _bucketRepository = bucketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Bucket>> Handle(
        CreateBucketCommand request,
        CancellationToken ct)
    {
        // Создаём ковш
        var bucket = Bucket.Create(request.Description);

        // Сохраняем в репозитории с использованием транзакции
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await _bucketRepository.AddAsync(bucket, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            await _unitOfWork.CommitTransactionAsync(ct);
            return Result.Success(bucket);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(ct);
            throw;
        }
    }
}