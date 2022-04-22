using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.Session;
[RequiresClaims(Permissions.Session_View)]
public class GetRequest : IRequest<Models.Session>
{
    public Guid? SessionId { get; set; }
    public Guid? UserId { get; set; }
    public bool AuthenticateSession { get; set; }
}
