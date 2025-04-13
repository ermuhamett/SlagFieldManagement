using SlagFieldManagement.Domain.Projection;

namespace SlagFieldManagement.Domain.Interfaces;

public interface ISlagFieldQueryRepository
{
    Task<List<SlagFieldStateProjection>> GetCurrentStatesAsync(CancellationToken ct);
}