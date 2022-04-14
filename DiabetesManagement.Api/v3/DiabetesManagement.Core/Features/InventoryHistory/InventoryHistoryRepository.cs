using DiabetesManagement.Core.Base;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.InventoryHistory;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Core.Features.InventoryHistory;

public class InventoryHistoryRepository : InventoryDbRepositoryBase<Models.InventoryHistory>, IInventoryHistoryRepository
{
    public InventoryHistoryRepository(InventoryDbContext context) : base(context)
    {

    }

    public async Task<IEnumerable<Models.InventoryHistory>> Get(DiabetesManagement.Features.Inventory.GetRequest request, CancellationToken cancellationToken)
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

    public async Task<int> GetLatestVersion(DiabetesManagement.Features.Inventory.GetRequest request, CancellationToken cancellationToken)
    {
        var includeQuery = DbSet
                .Include(i => i.Inventory);

        if (request.UserId.HasValue && !string.IsNullOrWhiteSpace(request.Key)
           && !string.IsNullOrWhiteSpace(request.Type))
        {
            return await includeQuery
                .Where(i => i.Inventory!.UserId == request.UserId && i.Inventory.Key == request.Key && i.Type == request.Type)
                .Select(i => i.Version)
                .MaxAsync(cancellationToken);
        }

        return 0;
    }
    public async Task<Models.InventoryHistory> Save(SaveCommand request, CancellationToken cancellationToken)
    {
        if (request.InventoryHistory == null)
        {
            throw new NullReferenceException();
        }

        var inventoryHistory = request.InventoryHistory;
        inventoryHistory.Created = DateTimeOffset.UtcNow;
        inventoryHistory.Hash = inventoryHistory.GetHash();

        DbSet.Add(inventoryHistory);

        if (request.CommitChanges)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }

        return inventoryHistory;
    }
}
