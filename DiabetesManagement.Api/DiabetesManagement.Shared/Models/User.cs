using DiabetesManagement.Shared.Base;
using System.ComponentModel.DataAnnotations;

namespace DiabetesManagement.Shared.Models
{
    [MessagePack.MessagePackObject(true)]
    public class User : DbModelBase
    {
        [Key]
        public Guid UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
        public string? Hash { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
    }
}
