using Inventory.Persistence;
using Inventory.Contracts;

namespace Inventory.Persistence.Base;

public abstract class InventoryDbRepositoryBase<T> : RepositoryBase<IInventoryDbContext, T>
    where T : class
{
    protected virtual Task<bool> IsMatch(T model, CancellationToken cancellationToken)
    {
        return RejectChanges;
    }

    protected InventoryDbRepositoryBase(IDbContextProvider dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
