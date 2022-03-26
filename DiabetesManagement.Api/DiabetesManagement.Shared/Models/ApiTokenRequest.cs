namespace DiabetesManagement.Shared.Models
{
    public class ApiTokenRequest
    {
        public Guid ApiTokenRequestId { get; set; }
        public Guid ApiTokenId { get; set; }
        public string? Token { get; set; }
        public DateTimeOffset Expires { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
