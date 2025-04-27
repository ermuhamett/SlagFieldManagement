using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Domain.Interfaces;

public interface IMaterialSettingsRepository
{
    Task<MaterialSettings?> GetByMaterialIdAsync(Guid materialId, CancellationToken ct = default);
    Task<List<MaterialSettings>> GetAllMaterials(CancellationToken ct = default);
    Task AddAsync(MaterialSettings settings, CancellationToken ct = default);
    void Update(MaterialSettings settings);
}