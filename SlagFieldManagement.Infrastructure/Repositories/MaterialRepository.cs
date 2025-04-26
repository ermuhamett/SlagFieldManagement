using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class MaterialRepository:Repository<Material>, IMaterialRepository
{
    public MaterialRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<List<Material>> GetAllAsync(CancellationToken ct = default)
    {
        return await DbContext.Set<Material>()
            .Where(m => !m.IsDelete)
            .ToListAsync(ct);
    }
}