using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldState;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldStateRepository:Repository<SlagFieldState>, ISlagFieldStateRepository
{
    public SlagFieldStateRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<SlagFieldState?> GetActiveStateAsync(
        Guid placeId, 
        CancellationToken cancellationToken = default)
    {
        // Стало
        return await DbContext.Set<SlagFieldState>()
            // 1) только для нужного места
            .Where(s => s.PlaceId == placeId)
            // 2) отбрасываем «удалённые» и «отмеченные как invalid»
            .Where(s => !s.IsDelete && s.State != StateFieldType.Invalid && s.State != StateFieldType.BucketRemoved)
            // 3) для нового ковша важен только факт «BucketPlaced»,
            //    а «BucketEmptied» мы тоже считаем ещё «активным» для опционального invalid
            .Where(s => s.State == StateFieldType.BucketPlaced || s.State == StateFieldType.BucketEmptied)
            // 4) сортируем по дате установки – самую свежую берем первой
            .OrderByDescending(s => s.StartDate)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public void Update(SlagFieldState state)
    {
        // Отмечаем сущность как изменённую
        DbContext.Set<SlagFieldState>().Update(state);
    }

    public async Task<Dictionary<Guid, Guid>> GetPlaceIdsForStatesAsync
        (List<Guid> stateIds, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<SlagFieldState>()
            .Where(s => stateIds.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => s.PlaceId, cancellationToken);
    }
}