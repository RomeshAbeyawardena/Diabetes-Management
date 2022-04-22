using Inventory.Contracts;

namespace Inventory.Features.InventoryHistory;

public class SaveCommand : ITransactionalCommand<Models.InventoryHistory>
{
    Models.InventoryHistory? ITransactionalCommand<Models.InventoryHistory>.Model => InventoryHistory;
    public Models.InventoryHistory? InventoryHistory { get; set; }
    public bool CommitChanges { get; set; }
}
