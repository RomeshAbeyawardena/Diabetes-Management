using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledger.Models;
[Table(nameof(Account)),
 MessagePack.MessagePackObject(true)]
public class Account
{
    [Key]
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    [Column("Reference_CS")]
    public string? ReferenceCaseSignature { get; set; }
    public bool Enabled { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
}
