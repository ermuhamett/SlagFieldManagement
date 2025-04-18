using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Domain.Interfaces;

public interface ISlagFieldStockRepository
{
    /// <summary>
    /// Получает запись SlagFieldStock по её уникальному идентификатору.
    /// </summary>
    Task<SlagFieldStock?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает запись SlagFieldStock по идентификатору состояния SlagFieldStateId.
    /// </summary>
    Task<SlagFieldStock?> GetByStateIdAsync(Guid stateId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавляет новую запись SlagFieldStock в хранилище.
    /// </summary>
    Task AddAsync(SlagFieldStock stock, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновляет существующую запись SlagFieldStock в хранилище.
    /// </summary>
    void Update(SlagFieldStock stock, CancellationToken cancellationToken = default);
}