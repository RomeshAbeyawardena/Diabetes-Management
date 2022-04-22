using DiabetesManagement.Attributes;
using MediatR;

namespace DiabetesManagement.Features.InventoryHistory;

[RequiresClaims(Permissions.InventoryHistory_View)]
public class GetRequest : IRequest<IEnumerable<Models.InventoryHistory>>
{
    public string? Intent { get; set; }

    public Guid? UserId { get; set; }
    public int? Version { get; set; }
    public bool GetLatest { get; set; }
    public string? Subject { get; set; }

    public Guid? InventoryHistoryId { get; set; }
}
