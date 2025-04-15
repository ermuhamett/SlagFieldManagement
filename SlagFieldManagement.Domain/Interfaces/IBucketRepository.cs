using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Domain.Interfaces;

public interface IBucketRepository
{
    Task<Bucket?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Bucket bucket, CancellationToken ct = default);
}