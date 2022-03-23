namespace DiabetesManagement.Shared.Models
{
    public class InventoryHistory : Inventory
    {
        public Guid InventoryHistoryId { get; set; }
        public int Version { get; set; }
        public string? Type { get; set; }
        public string? Items { get; set; }
        public DateTimeOffset InventoryHistoryCreated { get; set; }
    }
}
