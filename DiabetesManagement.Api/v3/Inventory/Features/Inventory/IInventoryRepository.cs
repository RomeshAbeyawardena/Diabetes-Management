using Inventory.Contracts;
using Inventory.Features.InventoryHistory;

namespace Inventory.Features.Inventory;

public interface IInventoryRepository : IRepository<IInventoryDbContext, Models.Inventory>
{
    Task<IEnumerable<Models.Inventory>> Get(GetRequest request, CancellationToken cancellationToken);
    Task<Models.Inventory> Save(SaveCommand postRequest, CancellationToken cancellationToken);
}
