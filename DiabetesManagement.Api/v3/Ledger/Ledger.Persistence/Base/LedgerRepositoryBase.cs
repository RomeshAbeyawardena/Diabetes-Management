using Inventory.Contracts;
using Inventory.Persistence.Base;
using Ledger.Contracts;

namespace Ledger.Persistence.Base;

public class LedgerRepositoryBase<T> : RepositoryBase<ILedgerDbContext, T>
    where T : class
{
    public LedgerRepositoryBase(IDbContextProvider dbContextProvider) : base(dbContextProvider)
    {
    }
}
