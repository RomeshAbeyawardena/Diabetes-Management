using DiabetesManagement.Attributes;
using MediatR;

namespace DiabetesManagement.Features.Inventory
{
    [RequiresClaims(Permissions.Inventory_Edit)]
    public class PostCommand : IRequest<Models.Inventory>
    {
        public Guid UserId { get; set; }
        public string? Subject { get; set; }
        public string? DefaultIntent { get; set; }
    }
}
