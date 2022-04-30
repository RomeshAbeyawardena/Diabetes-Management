using Inventory.Contracts;
using Ledger.Features.Account;
using Ledger.Persistence.Base;

namespace Ledger.Persistence.Repositories;

public class AccountRepository : LedgerRepositoryBase<Models.Account>, IAccountRepository
{
    public AccountRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
    {
    }

    public Task<Models.Account> Get(GetRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Models.Account> SaveAccount(SaveCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
