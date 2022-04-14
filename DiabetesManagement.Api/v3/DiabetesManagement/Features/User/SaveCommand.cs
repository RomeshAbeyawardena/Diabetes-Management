namespace DiabetesManagement.Features.User;

public class SaveCommand
{
    public Models.User? User { get; set; }
    public bool PrepareEncryptedFields { get; set; }
    public bool CommitChanges { get; set; }
}
