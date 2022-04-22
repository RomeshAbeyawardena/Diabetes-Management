using DiabetesManagement.Attributes;
using MediatR;

namespace DiabetesManagement.Features.Inventory
{
    [RequiresClaims(Permissions.Inventory_Edit)]
    public class PostCommand : IRequest<Models.Inventory>
    {
        public bool CommitChanges { get; set; } = true;
        public Guid? UserId { get; set; }
        public string? Subject { get; set; }
        public string? DefaultIntent { get; set; }
        public Models.Inventory? Inventory { get; set; }
    }
}
