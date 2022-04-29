using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.Function;

[RequiresClaims(Permissions.Anonymous_Access, Permissions.Function_Edit)]
public class PostCommand : IRequest<Models.Function>
{
    public string? Name { get; set; }
    public string? AccessToken { get; set; }
    public string? Path { get; set; }
}
