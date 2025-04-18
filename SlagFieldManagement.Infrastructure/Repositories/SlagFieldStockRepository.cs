using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldStockRepository:Repository<SlagFieldStock>, ISlagFieldStockRepository
{
    public SlagFieldStockRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    //TODO написано до Invalid(включительно)
    public async Task<SlagFieldStock?> GetByStateIdAsync(Guid stateId, CancellationToken ct = default)
    {
        return await DbContext.Set<SlagFieldStock>()
            .Where(s => s.SlagFieldStateId == stateId)
            .FirstOrDefaultAsync(ct);
    }

    public async Task AddAsync(SlagFieldStock state, CancellationToken ct) => await base.AddAsync(state, ct);

    public void Update(SlagFieldStock stock, CancellationToken ct = default) => base.Update(stock);
}