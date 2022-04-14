namespace DiabetesManagement.Features.Session
{
    public class SaveCommand
    {
        public Models.Session? Session { get; set; }
        public bool CommitChanges { get; set; }
        public bool ExpireSession { get; set; }
    }
}
