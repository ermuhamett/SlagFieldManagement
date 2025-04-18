namespace SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;

public interface ISlagFieldPlaceRepository
{
    Task<SlagFieldPlace?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(SlagFieldPlace place, CancellationToken cancellationToken = default);
    void Update(SlagFieldPlace place);

    Task<List<SlagFieldPlace>> GetAllAsync(CancellationToken cancellationToken = default);
}