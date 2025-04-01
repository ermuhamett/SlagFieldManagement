using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Domain.Entities;

public sealed class Bucket:Entity
{
    public string Description { get; private set; }
    public bool IsDelete { get; private set; }
    
    private Bucket(Guid id, string description):base(id)
    {
        Description = description;
        IsDelete = false;
    }

    public static Bucket Create(string description)
    {
        return new Bucket(Guid.NewGuid(), description);
    }
    
    public void MarkAsDeleted() => IsDelete = true;
}