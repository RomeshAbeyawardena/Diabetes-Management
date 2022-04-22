using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.AccessToken;
[RequiresClaims(Permissions.Anonymous_Access)]
public class ValidateRequest : IRequest<IDictionary<string, string>>
{
    public string? Token { get; set; }
}
