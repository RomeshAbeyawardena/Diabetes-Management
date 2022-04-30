using Inventory.Contracts;

namespace Ledger.Features.Ledger;

public class SaveCommand : ITransactionalCommand<Models.Ledger>
{
    Models.Ledger? ITransactionalCommand<Models.Ledger>.Model => Ledger;
    public Models.Ledger? Ledger { get; set; }
    public bool CommitChanges { get; set; }
}
