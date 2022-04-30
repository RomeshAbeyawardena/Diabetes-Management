using Inventory.Contracts;
using Ledger.Features.Ledger;
using Ledger.Persistence.Base;

namespace Ledger.Persistence.Repositories;

public class LedgerRepository : LedgerRepositoryBase<Models.Ledger>, ILedgerRepository
{
    public LedgerRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
    {
    }

    public Task<IEnumerable<Models.Ledger>> Get(GetRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Models.Ledger> SaveLedger(SaveCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
