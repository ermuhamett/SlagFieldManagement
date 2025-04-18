using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Infrastructure;

public sealed class ApplicationDbContext:DbContext, IUnitOfWork
{
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