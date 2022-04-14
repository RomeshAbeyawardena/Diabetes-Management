using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Core.Base;

[RegisterService(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
public abstract class InventoryDbRepositoryBase<T> : IRepository<InventoryDbContext, T>
    where T : class
{
    
    protected InventoryDbRepositoryBase(InventoryDbContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }

    public InventoryDbContext Context { get; }

    public DbSet<T> DbSet { get; }
}
