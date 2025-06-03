using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}