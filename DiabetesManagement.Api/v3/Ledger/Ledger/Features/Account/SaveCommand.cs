using Inventory.Contracts;

namespace Ledger.Features.Account;

public class SaveCommand : ITransactionalCommand<Models.Account>
{
    Models.Account? ITransactionalCommand<Models.Account>.Model => Account;
    public Models.Account? Account { get; set; }
    public bool CommitChanges { get; set; }
}
