namespace SlagFieldManagement.Domain.Aggregates.SlagFieldState;

public interface ISlagFieldStateRepository
{
    Task<SlagFieldState?> GetActiveStateAsync(Guid placeId, CancellationToken cancellationToken = default);
    Task AddAsync(SlagFieldState state, CancellationToken cancellationToken = default);
    void Update(SlagFieldState state);
    // Новый метод для получения маппинга stateId → placeId
    Task<Dictionary<Guid, Guid>> GetPlaceIdsForStatesAsync(List<Guid> stateIds, CancellationToken cancellationToken = default);
}