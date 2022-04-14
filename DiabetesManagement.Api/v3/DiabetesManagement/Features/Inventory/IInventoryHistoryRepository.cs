using DiabetesManagement.Contracts;

namespace DiabetesManagement.Features.Inventory;

public interface IInventoryHistoryRepository : IRepository<InventoryDbContext, Models.InventoryHistory>
{

}
