using Inventory.Contracts;

namespace Inventory.Features.AccessToken
{
    public class SaveCommand : ITransactionalCommand<Models.AccessToken>
    {
        Models.AccessToken? ITransactionalCommand<Models.AccessToken>.Model => AccessToken;
        public bool PrepareEncryptedFields { get; set; }
        public Models.AccessToken? AccessToken { get; set; }
        public bool CommitChanges { get; set; }
    }
}
