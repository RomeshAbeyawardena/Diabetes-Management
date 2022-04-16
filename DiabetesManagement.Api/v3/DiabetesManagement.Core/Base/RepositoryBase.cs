using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DiabetesManagement.Core.Base;

[RegisterService(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
public abstract class RepositoryBase<TDbContext, T> : IRepository<TDbContext, T>
    where TDbContext : DbContext
    where T : class
{
    private readonly DbSet<T> dbSet;
    private bool isReadonly;

    protected virtual bool IsReadOnly { set => isReadonly = value; }

    public RepositoryBase(IDbContextProvider dbContextProvider)
    {
        Context = dbContextProvider.GetDbContext<TDbContext>()!;
        dbSet = Context.Set<T>();
        isReadonly = true;
    }

    public TDbContext Context { get; }

    public IQueryable<T> DbSet => isReadonly ? dbSet.AsNoTracking() : dbSet;

    public virtual EntityEntry<T> Add(T entity)
    {
        return dbSet.Add(entity);
    }

    public virtual void Detach(EntityEntry<T> entityEntry)
    {
        entityEntry.State = EntityState.Detached;
    }

    public virtual Task<T?> FindAsync(Expression<Func<T, bool>> whereExpression, CancellationToken cancellationToken)
    {
        return DbSet.FirstOrDefaultAsync(whereExpression, cancellationToken);
    }

    public virtual EntityEntry<T> Update(T entity)
    {
        return dbSet.Update(entity);
    }
}
