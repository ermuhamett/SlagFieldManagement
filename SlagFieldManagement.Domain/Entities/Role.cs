using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Domain.Entities;

public sealed class Role:Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    // Приватный конструктор для EF Core
    private Role() { }
    
    // Фабричный метод
    public static Result<Role> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Role>(new Error("Role.EmptyName", "Название роли не может быть пустым."));

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        return Result.Success(role);
    }
}