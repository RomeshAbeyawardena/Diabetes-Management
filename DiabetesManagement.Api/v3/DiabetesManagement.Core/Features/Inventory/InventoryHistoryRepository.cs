using DiabetesManagement.Core.Base;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.Inventory;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Core.Features.Inventory;

public class InventoryHistoryRepository : InventoryDbRepositoryBase<Models.InventoryHistory>, IInventoryHistoryRepository
{
    public InventoryHistoryRepository(InventoryDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Models.InventoryHistory>> Get(GetRequest request, CancellationToken cancellationToken)
    {
        if (request.InventoryHistoryId.HasValue)
        {
            return await DbSet.Where(i => i.InventoryHistoryId == request.InventoryHistoryId).ToArrayAsync(cancellationToken);
        }

        var includeQuery = DbSet
                .Include(i => i.Inventory);

        if (request.UserId.HasValue && !string.IsNullOrWhiteSpace(request.Key)
            && !string.IsNullOrWhiteSpace(request.Type))
        {
            var query = includeQuery
                .Where(i => i.Inventory!.UserId == request.UserId && i.Inventory.Key == request.Key && i.Type == request.Type);

            if (request.Version.HasValue)
            {
                return await query.Where(i => i.Version == request.Version)
                    .ToArrayAsync(cancellationToken);
            }
            
            return await query.ToArrayAsync(cancellationToken);
        }

        return Array.Empty<Models.InventoryHistory>();
    }
}
