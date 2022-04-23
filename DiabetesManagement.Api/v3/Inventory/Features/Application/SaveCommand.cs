using Inventory.Contracts;

namespace Inventory.Features.Application
{
    public class SaveCommand : ITransactionalCommand<Models.Application>
    {
        public bool PrepareEncryptedFields { get; set; }
        Models.Application? ITransactionalCommand<Models.Application>.Model => Application;
        public Models.Application? Application { get; set; }
        public bool CommitChanges { get; set; }
    }
}
