using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.User;
[RequiresClaims(Permissions.User_Edit)]
public class PostCommand : IRequest<Models.User>
{
    public string? EmailAddress { get; set; }
    public string? DisplayName { get; set; }
    public string? Password { get; set; }
}
