using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Domain.Interfaces;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldStockRepository:Repository<SlagFieldStock>, ISlagFieldStockRepository
{
    public SlagFieldStockRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public Task<SlagFieldStock?> GetByStateIdAsync(Guid stateId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(SlagFieldStock state, CancellationToken ct) => await base.AddAsync(state, ct);
    
    public Task UpdateAsync(SlagFieldStock stock, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}