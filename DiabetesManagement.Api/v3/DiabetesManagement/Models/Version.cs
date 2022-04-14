namespace DiabetesManagement.Models;

public class Version
{
    public Guid InventoryHistoryId { get; set; }
    public Guid InventoryId { get; set; }
    public int Value { get; set; }
    public string? Type { get; set; }
    public DateTimeOffset Created { get; set; }
}
