using Inventory.Contracts;
using Ledger.Contracts;

namespace Ledger.Features.Account;

public interface IAccountRepository : IRepository<ILedgerDbContext, Models.Account>, IEncryptor<Models.Account>, IDecryptor<Models.Account>
{
    Task<Models.Account?> Get(GetRequest request, CancellationToken cancellationToken);
    Task<Models.Account> SaveAccount(SaveCommand command, CancellationToken cancellationToken);
}
