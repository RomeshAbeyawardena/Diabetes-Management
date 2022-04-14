using DiabetesManagement.Contracts;
using DiabetesManagement.Features.Inventory;

namespace DiabetesManagement.Features.InventoryHistory;

public interface IInventoryHistoryRepository : IRepository<InventoryDbContext, Models.InventoryHistory>
{
    Task<int> GetLatestVersion(GetRequest request, CancellationToken cancellationToken);
    Task<IEnumerable<Models.InventoryHistory>> Get(GetRequest request, CancellationToken cancellationToken);
    Task<Models.InventoryHistory> Save(SaveCommand request, CancellationToken cancellationToken);
}
