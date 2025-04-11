using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Domain.Entities;

public sealed class Material:Entity
{
    public string Name { get; private set; }
    public bool IsDelete { get; private set; }
    public MaterialSettings? Settings { get; private set; } // Ссылка на настройки
    
    
    private Material(Guid id, string name, MaterialSettings? settings):base(id)
    {
        Name = name;
        Settings = settings;
        IsDelete = false;
    }

    public static Material Create(string name, MaterialSettings settings)
    {
        return new Material(Guid.NewGuid(), name, settings);
    }
}