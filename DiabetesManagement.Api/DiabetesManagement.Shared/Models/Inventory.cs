using DiabetesManagement.Shared.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiabetesManagement.Shared.Models
{
    [MessagePack.MessagePackObject(true),
     Table("INVENTORY")]
    public class Inventory : DbModelBase
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
