namespace Ledger.Features.Ledger;

public interface ILedgerRepository
{
    Task<IEnumerable<Models.Ledger>> Get(GetRequest request, CancellationToken cancellationToken);
    Task<Models.Ledger> SaveLedger(SaveCommand command, CancellationToken cancellationToken);
}
