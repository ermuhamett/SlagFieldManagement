using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldPlaceRepository:Repository<SlagFieldPlace>, ISlagFieldPlaceRepository
{
    public SlagFieldPlaceRepository(ApplicationDbContext dbContext) : base(dbContext) {}

    public async Task<SlagFieldPlace?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<SlagFieldPlace>()
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDelete, cancellationToken);
    }
    
    public async Task<List<SlagFieldPlace>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<SlagFieldPlace>()
            .Where(p => !p.IsDelete)
            .ToListAsync(cancellationToken);
    }
    
    // Методы AddAsync и UpdateAsync можно не переопределять
    public async Task AddAsync(SlagFieldPlace place, CancellationToken ct) => await base.AddAsync(place, ct);
    public void Update(SlagFieldPlace place) => base.Update(place);
}