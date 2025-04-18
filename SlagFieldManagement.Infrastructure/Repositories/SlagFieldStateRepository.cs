using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Aggregates.SlagFieldState;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldStateRepository:Repository<SlagFieldState>, ISlagFieldStateRepository
{
    public SlagFieldStateRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<SlagFieldState?> GetActiveStateAsync(
        Guid placeId, 
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<SlagFieldState>()
            .Where(s => s.PlaceId == placeId && !s.IsDelete)
            .OrderByDescending(s => s.StartDate) // Берем последнее активное состояние
            .FirstOrDefaultAsync(cancellationToken);
    }

    // Используем базовые методы без переопределения
    //public async Task AddAsync(SlagFieldState state, CancellationToken ct) => await base.AddAsync(state, ct);
    //public void Update(SlagFieldState state, CancellationToken ct) => base.Update(state);
}