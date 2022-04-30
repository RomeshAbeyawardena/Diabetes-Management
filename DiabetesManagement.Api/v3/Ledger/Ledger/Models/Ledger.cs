using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledger.Models;

[Table(nameof(Account)),
 MessagePack.MessagePackObject(true)]
public class Ledger
{
    [Key]
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string? Reference { get; set; }
    [Column("Reference_CS")]
    public string? ReferenceCaseSignature { get; set; }
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
    public decimal PreviousBalance { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }

    public virtual Account? Account { get; set; }
}
