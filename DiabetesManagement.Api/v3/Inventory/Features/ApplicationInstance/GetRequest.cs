using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.ApplicationInstance
{
    [RequiresClaims(Permissions.Anonymous_Access)]
    public class GetRequest : IRequest<IEnumerable<Models.ApplicationInstance>>
    {
        public Guid? ApplicationInstanceId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
