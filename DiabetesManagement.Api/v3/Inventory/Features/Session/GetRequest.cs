using DiabetesManagement.Attributes;
using MediatR;

namespace DiabetesManagement.Features.Session;
[RequiresClaims(Permissions.Session_View)]
public class GetRequest : IRequest<Models.Session>
{
    public Guid? SessionId { get; set; }
    public Guid? UserId { get; set; }
    public bool AuthenticateSession { get; set; }
}
