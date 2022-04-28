using Inventory.Attributes;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Features.AccessToken;
[RequiresClaims(Permissions.Anonymous_Access)]
public class SignRequest : IRequest<string>
{
    public bool Validate { get; set; }
    [Required]
    public string? ApiIntent { get; set; }
    [Required]
    public string? ApiKey { get; set; }
    [Required]
    public string? ApiChallenge { get; set; }
}
