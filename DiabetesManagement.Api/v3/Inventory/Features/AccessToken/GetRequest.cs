using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.AccessToken
{
    [RequiresClaims(Permissions.Anonymous_Access, Permissions.AccessToken_View)]
    public class GetRequest : IRequest<Models.AccessToken>
    {
        public Guid Key { get; set; }
        public string? AccessToken { get; set; }
        public string? Intent { get; set; }
    }
}
