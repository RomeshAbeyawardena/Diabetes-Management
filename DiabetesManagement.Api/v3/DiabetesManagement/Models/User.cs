using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiabetesManagement.Models;

public class User
{
    [Key]
    public Guid UserId { get; set; }
    public string? DisplayName { get; set; }
    [Column("DISPLAYNAME_CS")]
    public string? DisplayNameCaseSignature { get; set; }

    public string? EmailAddress { get; set; }

    [Column("EMAILADDRESS_CS")]
    public string? EmailAddressCaseSignature { get; set; }
    public string? Password { get; set; }
    public string? Hash { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
}
