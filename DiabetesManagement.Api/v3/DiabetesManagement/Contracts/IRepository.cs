using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DiabetesManagement.Contracts;

public interface IRepository<TDbContext, T>
    where T: class
{
    TDbContext Context { get; }
    IQueryable<T> Query { get; }

    Task<T?> FindAsync(Expression<Func<T, bool>> whereExpression, CancellationToken cancellationToken);
    EntityEntry<T> Add(T entity);
    EntityEntry<T> Update(T entity);
    Task<EntityEntry<T>> Save(T model, CancellationToken cancellationToken);
    void Detach(EntityEntry<T> entityEntry);
}
