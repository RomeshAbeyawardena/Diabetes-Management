using System.ComponentModel.DataAnnotations.Schema;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    public class PutRequest
    {
        public string? Key { get; set; }
        [Column("DEFAULT_TYPE")]
        public string? DefaultType { get; set; }
        public string? Hash { get; set; }
        public DateTimeOffset? Modified { get; set; }
    }
}
