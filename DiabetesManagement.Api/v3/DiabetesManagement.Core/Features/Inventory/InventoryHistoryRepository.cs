using DiabetesManagement.Core.Base;
using DiabetesManagement.Features.Inventory;

namespace DiabetesManagement.Core.Features.Inventory;

public class InventoryHistoryRepository : InventoryDbRepositoryBase<Models.InventoryHistory>, IInventoryHistoryRepository
{
    public InventoryHistoryRepository(InventoryDbContext context) : base(context)
    {
    }
}
