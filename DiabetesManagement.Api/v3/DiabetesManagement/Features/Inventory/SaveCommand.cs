using DiabetesManagement.Contracts;

namespace DiabetesManagement.Features.Inventory;

public class SaveCommand : ITransactionalCommand<Models.Inventory>
{
    Models.Inventory? ITransactionalCommand<Models.Inventory>.Model => Inventory;
    public Models.Inventory? Inventory { get; set; }
    public bool CommitChanges { get; set; }
}
