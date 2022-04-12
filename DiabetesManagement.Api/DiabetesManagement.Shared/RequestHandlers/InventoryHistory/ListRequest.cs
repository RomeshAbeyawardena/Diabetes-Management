using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    public class ListRequest : IRequest<IEnumerable<Models.InventoryHistory>>
    {
        public Guid UserId { get; set; }
        public string? Key { get; set; }
        public string? Type { get; set; }
    }
}
