using DiabetesManagement.Shared.Base;
using DiabetesManagement.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    public class InventoryRepository : RepositoryBase<Models.Inventory>, IInventoryRepository
    {
        public InventoryRepository(InventoryDbContext inventoryDbContext) 
            : base(inventoryDbContext)
        {
        }

        public async Task<IEnumerable<Models.Inventory>> Get(GetRequest request, CancellationToken cancellationToken)
        {
            if (request.InventoryId.HasValue)
            {
                return await DbSet
                    .Where(t => t.InventoryId == request.InventoryId)
                    .ToArrayAsync(cancellationToken);
            }

            return await DbSet.Where(t => t.UserId == request.UserId 
                && t.Key == request.Key 
                && t.DefaultType == request.Type)
                .ToArrayAsync(cancellationToken);
        }

        public Task<Models.Inventory> Save(SaveCommand saveCommand, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
