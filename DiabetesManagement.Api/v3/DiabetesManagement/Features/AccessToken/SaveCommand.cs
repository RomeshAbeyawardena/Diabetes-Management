namespace DiabetesManagement.Features.AccessToken
{
    public class SaveCommand
    {
        public Models.AccessToken? AccessToken { get; set; }
        public bool CommitChanges { get; set; }
    }
}
