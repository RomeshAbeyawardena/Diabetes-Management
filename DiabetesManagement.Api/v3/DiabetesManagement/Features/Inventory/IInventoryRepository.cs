using DiabetesManagement.Contracts;

namespace DiabetesManagement.Features.Inventory;

public interface IInventoryRepository : IRepository<InventoryDbContext, Models.Inventory>
{
    Task<IEnumerable<Models.Inventory>> Get(GetRequest request, CancellationToken cancellationToken);
}
