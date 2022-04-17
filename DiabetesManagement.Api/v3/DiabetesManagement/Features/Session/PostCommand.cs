using DiabetesManagement.Attributes;
using MediatR;

namespace DiabetesManagement.Features.Session;

[RequiresClaims(Permissions.Session_Edit)]
public class PostCommand : IRequest<Models.Session>
{
    public Guid? SessionId { get; set; }
    public Guid? UserId { get; set; }
    public bool ExpireSession { get; set; }
}
