using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Domain.Entities;

public class User:Entity
{
    public Guid RoleId { get; private set; }
    public string UserName { get; private set; }
    public string PasswordHash { get; private set; }
    public string Email { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    // Навигационное свойство
    public Role Role { get; private set; }
    
    private User() { }
    
    // Фабричный метод
    public static Result<User> Create(
        Guid roleId,
        string userName,
        string passwordHash,
        string email)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return Result.Failure<User>(new Error("User.EmptyUsername", "Логин не может быть пустым."));

        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<User>(new Error("User.EmptyEmail", "Email не может быть пустым."));

        var user = new User
        {
            Id = Guid.NewGuid(),
            RoleId = roleId,
            UserName = userName,
            PasswordHash = passwordHash,
            Email = email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        return Result.Success(user);
    }

    // Метод для обновления данных
    public void Update(string email, DateTime updatedAt)
    {
        Email = email;
        UpdatedAt = updatedAt;
    }
}