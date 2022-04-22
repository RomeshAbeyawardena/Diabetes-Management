using DiabetesManagement.Contracts;

namespace DiabetesManagement.Features.User;

public class SaveCommand : ITransactionalCommand<Models.User>
{
    Models.User? ITransactionalCommand<Models.User>.Model => User;
    public Models.User? User { get; set; }
    public bool PrepareEncryptedFields { get; set; }
    public bool CommitChanges { get; set; }
}
