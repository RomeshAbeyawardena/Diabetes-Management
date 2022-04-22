
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiabetesManagement.Models;
[Table(nameof(ApplicationInstance)),
 MessagePack.MessagePackObject(true)]
public class ApplicationInstance
{
    [Key]
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public bool Enabled { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Expires { get; set; }
}
