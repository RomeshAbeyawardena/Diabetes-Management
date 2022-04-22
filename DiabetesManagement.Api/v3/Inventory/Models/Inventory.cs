using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiabetesManagement.Models
{
    [MessagePack.MessagePackObject(true),
     Table(nameof(Inventory))]
    public class Inventory
    {
        [Key]
        public Guid InventoryId { get; set; }
        public Guid UserId { get; set; }
        public string? Subject { get; set; }
        public string? DefaultIntent { get; set; }
        public string? Hash { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
    }
}
