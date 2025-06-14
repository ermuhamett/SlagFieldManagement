﻿using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Infrastructure.Repositories;

public abstract class Repository<T> where T : Entity
{
    protected readonly ApplicationDbContext DbContext;
    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await DbContext.AddAsync(entity, cancellationToken);
    }
    public void Update(T entity)
    {
        DbContext.Update(entity);
    }
    public void Delete(T entity)
    {
        DbContext.Remove(entity);
    }
}