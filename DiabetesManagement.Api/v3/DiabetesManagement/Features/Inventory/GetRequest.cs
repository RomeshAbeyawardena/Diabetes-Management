using DiabetesManagement.Attributes;
using MediatR;

namespace DiabetesManagement.Features.Inventory;

[RequiresClaims(Permissions.Inventory_View)]
public class GetRequest : IRequest<IEnumerable<Models.InventoryHistory>>
{
    public string? Key { get; set; }
    
    public Guid? UserId { get; set; }
    public int? Version { get; set; }
    public bool GetLatest { get; set; }
    public string? Type { get; set; }
    
    public Guid? InventoryHistoryId { get; set; }
}