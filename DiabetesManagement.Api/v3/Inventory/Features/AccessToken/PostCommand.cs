using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.AccessToken
{
    [RequiresClaims(Permissions.AccessToken_Edit)]
    public class PostCommand : IRequest<Models.AccessToken>
    {
        public Guid ApplicationId { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public DateTimeOffset? Expires { get; set; }
    }
}
