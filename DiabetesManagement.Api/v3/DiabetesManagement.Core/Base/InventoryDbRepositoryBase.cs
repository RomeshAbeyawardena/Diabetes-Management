using DiabetesManagement.Contracts;

namespace DiabetesManagement.Core.Base;

public abstract class InventoryDbRepositoryBase<T> : RepositoryBase<InventoryDbContext, T>
    where T : class
{
    protected InventoryDbRepositoryBase(IDbContextProvider dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
