using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    public class GetRequest : IRequest<Models.InventoryHistory>
    {
        public string? Key { get; set; }
        public string? Type { get; set; }
        public Guid UserId { get; set; }
        public int? Version { get; set; }
    }
}
