using DiabetesManagement.Contracts;

namespace DiabetesManagement.Features.Application
{
    public class SaveCommand : ITransactionalCommand<Models.Application>
    {
        Models.Application? ITransactionalCommand<Models.Application>.Model => Application;
        public Models.Application? Application { get; set; }
        public bool CommitChanges { get; set; }
    }
}
