using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.Function;

[RequiresClaims(Permissions.Anonymous_Access, Permissions.Function_Edit)]
public class PostCommand : IRequest<Models.Function>
{
    public string? AccessToken { get; set; }
    public decimal Complexity { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
}
