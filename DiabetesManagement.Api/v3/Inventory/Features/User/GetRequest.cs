using Inventory.Attributes;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Features.User;
[RequiresClaims(Permissions.User_View)]
public class GetRequest : IRequest<Models.User>
{
    public Guid? UserId { get; set; }
    [Required(ErrorMessage = "Email address is required")]
    public string? EmailAddress { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
    public bool AuthenticateUser { get; set; }
}
