using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldStockRepository:Repository<SlagFieldStock>, ISlagFieldStockRepository
{
    public SlagFieldStockRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    
    public async Task<SlagFieldStock?> GetByStateIdAsync(Guid stateId, CancellationToken ct = default)
    {
        return await DbContext.Set<SlagFieldStock>()
            .Where(s => s.SlagFieldStateId == stateId)
            .FirstOrDefaultAsync(ct);
    }
}