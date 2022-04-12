using DiabetesManagement.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers.InventoryHistory
{
    using Models = Shared.Models;

    public class ListRequest : IRequest<IEnumerable<Models.InventoryHistory>>
    {
        public Guid UserId { get; set; }
        public string? Key { get; set; }
        public string? Type { get; set; }
    }
}
