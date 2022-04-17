namespace DiabetesManagement.Features.Application
{
    public class SaveCommand
    {
        public Models.Application? Application { get; set; }
        public bool CommitChanges { get; set; }
    }
}
