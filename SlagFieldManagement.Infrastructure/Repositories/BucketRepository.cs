using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class BucketRepository:Repository<Bucket>, IBucketRepository
{
    public BucketRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    
    public async Task<List<Bucket>> GetAllBuckets(CancellationToken ct = default)
    {
        return await DbContext.Set<Bucket>()
            .Where(b => !b.IsDelete)
            .ToListAsync(ct);
    }

    public async Task DeleteAsync(Bucket bucket, CancellationToken ct = default)
    {
        bucket.MarkAsDeleted();
        Update(bucket);
        await DbContext.SaveChangesAsync(ct);
    }
}