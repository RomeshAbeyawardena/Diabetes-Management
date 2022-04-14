using MediatR;

namespace DiabetesManagement.Features.Inventory;

public class GetRequest : IRequest<IEnumerable<Models.Inventory>>
{
    public string? Key { get; set; }
    public Guid? UserId { get; set; }
    public string? Version { get; set; }
    public string? Type { get; set; }
    public Guid? InventoryHistoryId { get; set; }
}