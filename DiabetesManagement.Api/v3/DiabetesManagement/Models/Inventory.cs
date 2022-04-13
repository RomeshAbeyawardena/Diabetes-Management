using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiabetesManagement.Models
{
    [MessagePack.MessagePackObject(true),
     Table("INVENTORY_HISTORY")]
    public class Inventory
    {
        [Key]
        public Guid InventoryId { get; set; }
        public Guid UserId { get; set; }
        public string? Key { get; set; }
        [Column("DEFAULT_TYPE")]
        public string? DefaultType { get; set; }
        public string? Hash { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
    }
}
