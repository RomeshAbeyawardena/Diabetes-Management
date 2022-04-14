using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiabetesManagement.Models
{
    [MessagePack.MessagePackObject(true),
     Table("INVENTORY_HISTORY")]
    public class InventoryHistory
    {
        [Column("INVENTORY_HISTORYID"), Key]
        public Guid InventoryHistoryId { get; set; }
        public Guid InventoryId { get; set; }
        public int Version { get; set; }
        public string? Type { get; set; }
        public string? Items { get; set; }
        public string? Hash { get; set; }
        public DateTimeOffset Created { get; set; }

        public virtual Inventory? Inventory { get; set; }
    }
}
