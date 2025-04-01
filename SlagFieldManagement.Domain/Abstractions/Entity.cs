namespace SlagFieldManagement.Domain.Abstractions;

public abstract class Entity
{
    protected Entity(Guid id) => Id = id;
    
    protected Entity() { }
    /// <summary>
    /// Уникальный идентификатор сущности. Используется init для предотвращения изменений после создания.
    /// </summary>
    public Guid Id { get; init; }
}