using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models;
[Table(nameof(ApplicationInstance)),
 MessagePack.MessagePackObject(true)]
public class ApplicationInstance
{
    [Key]
    public Guid ApplicationInstanceId { get; set; }
    public Guid ApplicationId { get; set; }
    public bool Enabled { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Expires { get; set; }
}
