namespace DiabetesManagement.Features.InventoryHistory;

public class SaveCommand
{
    public Models.InventoryHistory? InventoryHistory { get; set; }
    public bool CommitChanges { get; set; }
}
