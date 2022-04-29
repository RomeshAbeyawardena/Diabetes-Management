using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.Function
{
    [RequiresClaims(Permissions.Anonymous_Access, Permissions.Function_View)]
    public class GetRequest : IRequest<Models.Function>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
    }
}
