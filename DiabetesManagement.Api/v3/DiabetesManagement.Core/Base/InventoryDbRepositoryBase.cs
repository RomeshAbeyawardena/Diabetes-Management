using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DiabetesManagement.Core.Base;

[RegisterService(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
public abstract class InventoryDbRepositoryBase<T> : IRepository<InventoryDbContext, T>
    where T : class
{
    private readonly DbSet<T> dbSet;
    private bool isReadonly;

    protected bool IsReadOnly { set => isReadonly = value; }

    protected InventoryDbRepositoryBase(InventoryDbContext context)
    {
        Context = context;
        dbSet = Context.Set<T>();
        isReadonly = true;
    }

    public InventoryDbContext Context { get; }

    public IQueryable<T> DbSet => isReadonly ? dbSet.AsNoTracking() : dbSet;

    public Task<T?> FindAsync(Expression<Func<T, bool>> whereExpression, CancellationToken cancellationToken)
    {
        return DbSet.FirstOrDefaultAsync(whereExpression, cancellationToken);
    }

    public EntityEntry<T> Add(T entity)
    {
        return dbSet.Add(entity);
    }

    public EntityEntry<T> Update(T entity)
    {
        return dbSet.Update(entity);
    }

    public void Detach(EntityEntry<T> entityEntry)
    {
        entityEntry.State = EntityState.Detached;
    }
}
