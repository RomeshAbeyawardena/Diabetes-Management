using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    public class SaveRequest : IRequest<Guid>
    {
        public Models.Inventory? Inventory { get; set; }
        public bool ThrowIfInventoryRowExists { get; set; }
        public bool CommitOnCompletion { get; set; }
    }
}
