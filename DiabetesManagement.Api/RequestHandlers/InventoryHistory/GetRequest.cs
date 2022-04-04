using DiabetesManagement.Shared.Contracts;
using System;

namespace DiabetesManagement.Api.RequestHandlers.InventoryHistory
{
    public class GetRequest : IRequest<Shared.Models.InventoryHistory>
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public Guid UserId { get; set; }
        public int? Version { get; set; }
    }
}
