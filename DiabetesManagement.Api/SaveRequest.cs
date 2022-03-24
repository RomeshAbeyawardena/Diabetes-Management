using DiabetesManagement.Shared.Contracts;
using DiabetesManagement.Shared.Models;
using System;

namespace DiabetesManagement.Api
{
    public class SaveRequest : IRequest<InventoryHistory>
    {
        public Guid UserId { get; set; }
        public int Version { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public string Items { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
