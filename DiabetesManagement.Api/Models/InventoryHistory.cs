using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.Models
{
    public class InventoryHistory : Inventory
    {
        public Guid InventoryHistoryId { get; set; }
        public int Version { get; set; }
        public string Items { get; set; }
        public DateTimeOffset InventoryHistoryCreated { get; set; }
    }
}
