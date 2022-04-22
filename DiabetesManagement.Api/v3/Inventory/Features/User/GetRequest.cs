using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.User;
[RequiresClaims(Permissions.User_View)]
public class GetRequest : IRequest<Models.User>
{
    public Guid? UserId { get; set; }
    public string? EmailAddress { get; set; }
    public string? Password { get; set; }
    public bool AuthenticateUser { get; set; }
}
