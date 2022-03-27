using DiabetesManagement.Shared.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiabetesManagement.Shared.Models
{
    [MessagePack.MessagePackObject(true),
     Table("INVENTORY_HISTORY")]
    public class InventoryHistory : DbModelBase
    {
        [Column("INVENTORY_HISTORYID"), Key]
        public Guid InventoryHistoryId { get; set; }
        public Guid InventoryId { get; set; }
        public int Version { get; set; }
        public string? Type { get; set; }
        public string? Items { get; set; }
        public string? Hash { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
