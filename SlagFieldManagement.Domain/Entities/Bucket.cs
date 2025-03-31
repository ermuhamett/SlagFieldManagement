namespace SlagFieldManagement.Domain.Entities;

public sealed class Bucket
{
    public Guid Id { get; private set; }
    public string Description { get; private set; }
    public bool IsDelete { get; private set; }
    
    private Bucket(Guid id, string description)
    {
        Id = id;
        Description = description;
        IsDelete = false;
    }

    public static Bucket Create(string description)
    {
        var bucket = new Bucket(Guid.NewGuid(), description);
        return bucket;
    }
}