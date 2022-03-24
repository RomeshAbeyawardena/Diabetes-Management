namespace DiabetesManagement.Shared.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string? Username { get; set; }
        public string? EmailAddress { get; set; }
        public string? Hash { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
    }
}
