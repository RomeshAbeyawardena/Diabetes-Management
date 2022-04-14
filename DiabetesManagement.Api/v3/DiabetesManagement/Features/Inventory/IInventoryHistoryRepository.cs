using DiabetesManagement.Contracts;

namespace DiabetesManagement.Features.Inventory;

public interface IInventoryHistoryRepository : IRepository<InventoryDbContext, Models.InventoryHistory>
{
    Task<IEnumerable<Models.InventoryHistory>> Get(GetRequest request, CancellationToken cancellationToken);
}
