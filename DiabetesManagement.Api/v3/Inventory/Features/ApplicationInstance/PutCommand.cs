using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.ApplicationInstance;
[RequiresClaims(Permissions.Anonymous_Access, Permissions.ApplicationInstance_Edit)]
public class PutCommand : IRequest<Models.ApplicationInstance>
{
    public Models.ApplicationInstance? ApplicationInstance { get; set; }
    public bool CommitChanges { get; set; }
}
