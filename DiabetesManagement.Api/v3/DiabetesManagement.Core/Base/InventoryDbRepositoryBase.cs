using DiabetesManagement.Contracts;

namespace DiabetesManagement.Core.Base;

public abstract class InventoryDbRepositoryBase<T> : RepositoryBase<InventoryDbContext, T>
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
