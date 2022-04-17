using DiabetesManagement.Contracts;

namespace DiabetesManagement.Features.AccessToken
{
    public class SaveCommand : ITransactionalCommand<Models.AccessToken>
    {
        Models.AccessToken? ITransactionalCommand<Models.AccessToken>.Model => AccessToken;
        public Models.AccessToken? AccessToken { get; set; }
        public bool CommitChanges { get; set; }
    }
}
