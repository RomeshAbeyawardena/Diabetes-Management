using DiabetesManagement.Shared.Contracts;
using Models = DiabetesManagement.Shared.Models;
using System;

namespace DiabetesManagement.Api.RequestHandlers.InventoryHistory
{
    public class SaveRequest : IRequest<Models.InventoryHistory>
    {
        public Guid UserId { get; set; }
        public int Version { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public string Items { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
