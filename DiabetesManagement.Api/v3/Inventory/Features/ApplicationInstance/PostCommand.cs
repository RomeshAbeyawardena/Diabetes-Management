using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.ApplicationInstance;

[RequiresClaims(Permissions.Anonymous_Access)]
public class PostCommand : IRequest<Models.ApplicationInstance>
{
    public Guid ApplicationId { get; set; }
    public DateTimeOffset Expires { get; set; }
}
