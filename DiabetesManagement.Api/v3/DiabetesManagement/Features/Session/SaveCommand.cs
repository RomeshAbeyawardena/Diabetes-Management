using DiabetesManagement.Contracts;

namespace DiabetesManagement.Features.Session
{
    public class SaveCommand : ITransactionalCommand<Models.Session>
    {
        Models.Session? ITransactionalCommand<Models.Session>.Model => Session;
        public Models.Session? Session { get; set; }
        public bool CommitChanges { get; set; }
        public bool ExpireSession { get; set; }
    }
}
