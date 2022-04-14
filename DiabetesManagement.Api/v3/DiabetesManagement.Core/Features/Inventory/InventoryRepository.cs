using DiabetesManagement.Core.Base;
using DiabetesManagement.Features.Inventory;

namespace DiabetesManagement.Core.Features.Inventory;

public class InventoryRepository : InventoryDbRepositoryBase<Models.Inventory>, IInventoryRepository
{
    public InventoryRepository(InventoryDbContext context) : base(context)
    {
    }

    public Task<IEnumerable<Models.Inventory>> Get(GetRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
