using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    public class GetRequest : IRequest<IEnumerable<Models.Inventory>>
    {
        public Guid? InventoryId { get; set; }
        public string? Type { get; set; }
        public string? Key { get; set; }
        public Guid UserId { get; set; }
    }
}
