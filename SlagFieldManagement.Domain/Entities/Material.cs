using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Domain.Entities;

public sealed class Material:Entity
{
    public string Name { get; private set; }
    public bool IsDelete { get; private set; }
    
    private Material(Guid id, string name):base(id)
    {
        Name = name;
        IsDelete = false;
    }

    public static Material Create(string name) => new Material(Guid.NewGuid(), name);
}