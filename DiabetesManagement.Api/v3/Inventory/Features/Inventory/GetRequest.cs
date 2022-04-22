using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.Inventory;

[RequiresClaims(Permissions.Inventory_View)]
public class GetRequest : IRequest<IEnumerable<Models.Inventory>>
{
    public string? Subject { get; set; }
    public string? Intent { get; set; }
    public Guid? InventoryId { get; set; }
    public Guid? UserId { get; set; }
}