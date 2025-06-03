using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldState;
using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Infrastructure.EventStores;

namespace SlagFieldManagement.Infrastructure;

public sealed class ApplicationDbContext:DbContext, IUnitOfWork
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Bucket> Buckets { get; set; }
    public DbSet<MaterialSettings> MaterialSettings { get; set; }
    public DbSet<SlagFieldState> SlagFieldStates { get; set; }
    public DbSet<SlagFieldPlaceEvent> SlagFieldPlaceEvents { get; set; }
    public DbSet<SlagFieldStateEvent> SlagFieldStateEvents { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        await Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        await Database.CommitTransactionAsync(ct);
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        await Database.RollbackTransactionAsync(ct);
    }
}
