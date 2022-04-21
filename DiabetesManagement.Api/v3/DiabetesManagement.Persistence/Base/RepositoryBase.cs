using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;
using DiabetesManagement.Extensions.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace DiabetesManagement.Core.Base;

[RegisterService(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
public abstract class RepositoryBase<TDbContext, T> : IRepository<TDbContext, T>
    where TDbContext : IDbContext
    where T : class
{
    private readonly DbSet<T> dbSet;
    private bool isReadonly;

    private static PropertyInfo? GetIdProperty()
    {
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<KeyAttribute>() != null)
            {
                return property;
            }
        }

        return default!;
    }

    protected Task<bool> AcceptChanges => Task.FromResult(true);
    protected Task<bool> RejectChanges => Task.FromResult(false);

    protected virtual bool IsReadOnly { set => isReadonly = value; }

    protected async virtual Task<T> Save<TRequest>(TRequest request, CancellationToken cancellationToken)
        where TRequest : ITransactionalCommand<T>
    {
        if (request.Model == null)
        {
            throw new NullReferenceException();
        }

        var entityEntry = await Save(request.Model, cancellationToken);
        
        if (entityEntry != null && request.CommitChanges)
        {
            await Context.SaveChangesAsync(cancellationToken);
            Detach(entityEntry);
        }
        
        return request.Model;
    }

    protected virtual Task<bool> Add(T model, CancellationToken cancellationToken)
    {
        return AcceptChanges;
    }

    protected virtual Task<bool> Update(T model, CancellationToken cancellationToken)
    {
        return AcceptChanges;
    }

    public RepositoryBase(IDbContextProvider dbContextProvider)
    {
        Context = dbContextProvider.GetDbContext<TDbContext>()!;
        dbSet = Context.Set<T>();
        isReadonly = true;
    }

    public TDbContext Context { get; }

    public IQueryable<T> Query => isReadonly ? dbSet.AsNoTracking() : dbSet;

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
        return Query.FirstOrDefaultAsync(whereExpression, cancellationToken);
    }

    public virtual EntityEntry<T> Update(T entity)
    {
        return dbSet.Update(entity);
    }

    public async Task<EntityEntry<T>> Save(T model, CancellationToken cancellationToken)
    {
        PropertyInfo? idProperty = GetIdProperty();
        if (idProperty != null && model != null) 
        {
            var idValue = idProperty.GetValue(model);

            if(idValue == null || idValue.IsDefaultValue())
            {
                if(await Add(model, cancellationToken))
                {
                    return Add(model);
                }
            }
            else
            {
                if (await Update(model, cancellationToken))
                {
                    return Update(model);
                }
            }
        }

        return default!;
    }
}
