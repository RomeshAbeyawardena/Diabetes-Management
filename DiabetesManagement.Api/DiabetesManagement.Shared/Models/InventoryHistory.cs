namespace DiabetesManagement.Shared.Models
{
    [MessagePack.MessagePackObject(true)]
    public class InventoryHistory : Inventory
    {
        public Guid InventoryHistoryId { get; set; }
        public int Version { get; set; }
        public string? Type { get; set; }
        public string? Items { get; set; }
        public string? InventoryHistoryHash { get; set; }
        public DateTimeOffset InventoryHistoryCreated { get; set; }
    }
}
