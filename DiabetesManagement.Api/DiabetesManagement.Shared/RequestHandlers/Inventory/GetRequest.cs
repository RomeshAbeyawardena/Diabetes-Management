using DiabetesManagement.Shared.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    public class GetRequest : IRequest<Models.Inventory>
    {
        public Guid? InventoryId { get; set; }
        [Column("DefaultType")]
        public string? Type { get; set; }
        public string? Key { get; set; }
        public Guid UserId { get; set; }
    }
}
