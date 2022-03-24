using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    public class GetRequest : IRequest<Models.Inventory>
    {
        public Guid? InventoryId { get; set; }
        public string? Type { get; set; }
        public string? Key { get; set; }
        public Guid UserId { get; set; }
    }
}
