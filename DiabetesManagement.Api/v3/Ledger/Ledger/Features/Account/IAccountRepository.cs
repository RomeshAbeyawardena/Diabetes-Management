namespace Ledger.Features.Account;

public interface IAccountRepository
{
    Task<Models.Account> Get(GetRequest request, CancellationToken cancellationToken);
    Task<Models.Account> SaveAccount(SaveCommand command, CancellationToken cancellationToken);
}
