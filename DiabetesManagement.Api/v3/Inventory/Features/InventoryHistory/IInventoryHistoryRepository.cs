using Inventory.Contracts;

namespace Inventory.Features.InventoryHistory;

public interface IInventoryHistoryRepository : IRepository<IInventoryDbContext, Models.InventoryHistory>
{
    Task<int> GetLatestVersion(GetVersionRequest request, CancellationToken cancellationToken);
    Task<IEnumerable<Models.InventoryHistory>> Get(GetRequest request, CancellationToken cancellationToken);
    Task<Models.InventoryHistory> Save(SaveCommand request, CancellationToken cancellationToken);
}
