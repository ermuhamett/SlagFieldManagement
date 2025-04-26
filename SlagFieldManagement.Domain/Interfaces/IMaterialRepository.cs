using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Domain.Interfaces;

public interface IMaterialRepository
{
    Task<Material?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Material>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Material material, CancellationToken ct = default);
    void Update(Material material);
    void Delete(Material material);
}