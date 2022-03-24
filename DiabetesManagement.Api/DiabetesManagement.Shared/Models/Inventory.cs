namespace DiabetesManagement.Shared.Models
{
    [MessagePack.MessagePackObject(true)]
    public class Inventory
    {
        public Guid InventoryId { get; set; }
        public Guid UserId { get; set; }
        public string? Key { get; set; }
        public string? DefaultType { get; set; }
        public string? Hash { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
    }
}
