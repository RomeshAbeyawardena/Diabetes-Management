using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiabetesManagement.Models;

[Table(nameof(Application)),
 MessagePack.MessagePackObject(true)]
public class Application
{
    [Key]
    public Guid ApplicationId { get; set; }
    public Guid UserId { get; set; }
    
    public string? Name { get; set; }
    
    [Column("Name_CS")]
    public string? NameCaseSignature { get; set; }

    public string? DisplayName { get; set; }
    
    [Column("DisplayName_CS")]
    public string? DisplayNameCaseSignature { get; set; }
    
    public string? Intent { get; set; }
    public bool? Enabled { get; set; }
    public string? Hash { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
    public DateTimeOffset? Expires { get; set; }

    public virtual User? User { get; set; }
}
