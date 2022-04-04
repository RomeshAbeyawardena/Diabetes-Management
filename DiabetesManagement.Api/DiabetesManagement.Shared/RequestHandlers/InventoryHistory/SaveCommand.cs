using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    public class SaveCommand : IRequest<Guid>
    {
        public Models.InventoryHistory? InventoryHistory { get; set; }
        public bool ThrowIfInventoryRowExists { get; set; }
        public bool CommitOnCompletion { get; set; }
    }
}
