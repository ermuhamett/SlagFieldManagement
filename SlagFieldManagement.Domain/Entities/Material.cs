using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.ValueObjects;

namespace SlagFieldManagement.Domain.Entities;

public sealed class Material:Entity
{
    public string Name { get; private set; }
    public bool IsDelete { get; private set; }
    public MaterialSettings Settings { get; private set; }
    
    private Material(Guid id, string name, MaterialSettings settings):base(id)
    {
        Id = id;
        Name = name;
        Settings = settings;
        IsDelete = false;
    }

    public static Material Create(string name, MaterialSettings settings)
    {
        return new Material(Guid.NewGuid(), name, settings);
    }
}