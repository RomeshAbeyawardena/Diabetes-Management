using DiabetesManagement.Shared.Base;

namespace DiabetesManagement.Shared.Models
{
    [MessagePack.MessagePackObject(true)]
    public class InventoryHistory : DbModelBase
    {
        public Guid InventoryHistoryId { get; set; }
        public Guid InventoryId { get; set; }
        public int Version { get; set; }
        public string? Type { get; set; }
        public string? Items { get; set; }
        public string? Hash { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
