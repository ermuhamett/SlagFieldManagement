using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class MaterialSettingsRepository:Repository<MaterialSettings>, IMaterialSettingsRepository
{
    public MaterialSettingsRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<MaterialSettings?> GetByMaterialIdAsync(
        Guid materialId, 
        CancellationToken ct = default)
    {
        return await DbContext.Set<MaterialSettings>()
            .FirstOrDefaultAsync(s => s.MaterialId == materialId && !s.IsDelete, ct);
    }

    public async Task<List<MaterialSettings>> GetAllMaterials(CancellationToken ct = default)
    {
        return await DbContext.Set<MaterialSettings>()
            .Where(ms => !ms.IsDelete)
            .ToListAsync(ct);
    }
}