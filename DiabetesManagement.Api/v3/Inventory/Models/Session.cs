using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    [MessagePack.MessagePackObject(true),
     Table(nameof(Session))]
    public class Session
    {
        [Key]
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Expires { get; set; }
        public bool Enabled { get; set; }
        public virtual User? User { get; set; }
    }
}
