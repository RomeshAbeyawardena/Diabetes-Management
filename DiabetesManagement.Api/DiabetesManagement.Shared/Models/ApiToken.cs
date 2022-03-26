using DiabetesManagement.Shared.Base;

namespace DiabetesManagement.Shared.Models
{
    public class ApiToken : DbModelBase
    {
        public Guid ApiTokenId { get; set; }
        public string? Key { get; set; }
        public string? Secret { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
