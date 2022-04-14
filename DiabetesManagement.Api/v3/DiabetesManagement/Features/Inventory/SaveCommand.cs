namespace DiabetesManagement.Features.Inventory;

public class SaveCommand
{
    public Models.Inventory? Inventory { get; set; }
    public bool CommitChanges { get; set; }
}
