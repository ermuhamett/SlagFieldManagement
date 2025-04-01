namespace SlagFieldManagement.Domain.Entities;

public sealed class Material
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool IsDelete { get; private set; }

    private Material(Guid id, string name)
    {
        Id = id;
        Name = name;
        IsDelete = false;
    }

    public static Material Create(string name)
    {
        var material = new Material(Guid.NewGuid(), name);
        return material;
    }
}