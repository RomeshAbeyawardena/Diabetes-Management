using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiabetesManagement.Models;

[Table("AccessToken_Claim")]
public class AccessTokenClaim
{
    [Key, Column("AccessToken_ClaimId")]
    public Guid AccessTokenClaimId { get; set; }
    public Guid AccessTokenId { get; set; }
    public string? Claim { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Expires { get; set; }

    public virtual AccessToken? AccessToken { get; set; }
}
