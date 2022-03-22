using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.Models
{
    public class Inventory
    {
        public Guid InventoryId { get; set; }
        public string Key { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
    }
}
