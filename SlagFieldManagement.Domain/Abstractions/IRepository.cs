using System.Linq.Expressions;

namespace SlagFieldManagement.Domain.Abstractions;

public interface IRepository<T> where T:Entity
{
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}