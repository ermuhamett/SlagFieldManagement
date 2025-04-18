using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class BucketRepository:Repository<Bucket>, IBucketRepository
{
    public BucketRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    
    public async Task<Bucket?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await base.GetByIdAsync(id, ct);
    }

    public async Task AddAsync(Bucket bucket, CancellationToken ct = default)
    {
        await base.AddAsync(bucket, ct);
    }
}