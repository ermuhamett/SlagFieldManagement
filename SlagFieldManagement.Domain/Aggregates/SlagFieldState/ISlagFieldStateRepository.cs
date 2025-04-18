namespace SlagFieldManagement.Domain.Aggregates.SlagFieldState;

public interface ISlagFieldStateRepository
{
    Task<SlagFieldState?> GetActiveStateAsync(Guid placeId, CancellationToken cancellationToken = default);
    Task AddAsync(SlagFieldState state, CancellationToken cancellationToken = default);
    void Update(SlagFieldState state);
}