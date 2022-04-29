using MediatR;

namespace Inventory.Features.Function
{
    public class GetRequest : IRequest<Models.Function>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
    }
}
