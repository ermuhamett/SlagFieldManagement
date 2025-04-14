namespace SlagFieldManagement.Domain.Aggregates.SlagFieldState;

public interface ISlagFieldStateRepository
{
    Task<SlagFieldState?> GetActiveStateAsync(Guid placeId, CancellationToken cancellationToken = default);
    Task AddAsync(SlagFieldState state, CancellationToken cancellationToken = default);
    Task UpdateAsync(SlagFieldState state, CancellationToken cancellationToken = default);
}