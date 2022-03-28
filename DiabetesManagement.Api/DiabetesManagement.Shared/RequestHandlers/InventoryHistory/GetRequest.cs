using DiabetesManagement.Shared.Contracts;
using System.Runtime.Serialization;

namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    public class GetRequest : IRequest<Models.InventoryHistory>
    {
        public Guid? InventoryHistoryId { get; set; }
        public Guid? InventoryId { get; set; }
        public string? Key { get; set; }
        public string? Type { get; set; }
        public Guid UserId { get; set; }
        public int? Version { get; set; }

        [IgnoreDataMember]
        public bool IsLatest { get; set; }
    }
}
