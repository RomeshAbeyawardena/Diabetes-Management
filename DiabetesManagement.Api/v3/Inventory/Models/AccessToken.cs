using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models;

[Table(nameof(AccessToken)),
     MessagePack.MessagePackObject(true)]
public class AccessToken
{
    [Key]
    public Guid AccessTokenId { get; set; }
    public Guid ApplicationId { get; set; }
    public string? Key { get; set; }
    [Column("Key_CS")]
    public string? KeySignature { get; set; }
    public string? Value { get; set; }
    public bool? Enabled { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Expires { get; set; }

    public virtual Application? Application { get; set; }
    public virtual ICollection<AccessTokenClaim>? AccessTokenClaims { get; set; }
}
