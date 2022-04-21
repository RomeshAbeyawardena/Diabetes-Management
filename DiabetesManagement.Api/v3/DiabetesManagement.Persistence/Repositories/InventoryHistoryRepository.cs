using DiabetesManagement.Contracts;
using DiabetesManagement.Core.Base;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.InventoryHistory;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Persistence.Repositories;

public class InventoryHistoryRepository : InventoryDbRepositoryBase<Models.InventoryHistory>, IInventoryHistoryRepository
{
    private readonly IClockProvider clockProvider;

    protected override Task<bool> Add(Models.InventoryHistory inventoryHistory, CancellationToken cancellationToken)
    {
        inventoryHistory.Created = clockProvider.Clock.UtcNow;
        inventoryHistory.Hash = inventoryHistory.GetHash();
        return Task.FromResult(true);
    }

    public InventoryHistoryRepository(IDbContextProvider context, IClockProvider clockProvider) : base(context)
    {
        this.clockProvider = clockProvider;
    }

    public async Task<IEnumerable<Models.InventoryHistory>> Get(GetRequest request, CancellationToken cancellationToken)
    {
        if (request.InventoryHistoryId.HasValue)
        {
            return await Query.Where(i => i.InventoryHistoryId == request.InventoryHistoryId).ToArrayAsync(cancellationToken);
        }

        var includeQuery = Query
                .Include(i => i.Inventory);

        if (request.UserId.HasValue && !string.IsNullOrWhiteSpace(request.Subject)
            && !string.IsNullOrWhiteSpace(request.Intent))
        {
            var query = includeQuery
                .Where(i => i.Inventory!.UserId == request.UserId && i.Inventory.Subject == request.Subject && i.Intent == request.Intent);

            if (request.Version.HasValue)
            {
                return await query.Where(i => i.Version == request.Version)
                    .ToArrayAsync(cancellationToken);
            }

            if (request.GetLatest)
            {
                query = query.OrderByDescending(a => a.Version);
                var firstEntry = await query.FirstOrDefaultAsync(cancellationToken);
                return new Models.InventoryHistory[] { firstEntry! };
            }

            return await query.ToArrayAsync(cancellationToken);
        }

        return Array.Empty<Models.InventoryHistory>();
    }

    public async Task<int> GetLatestVersion(GetVersionRequest request, CancellationToken cancellationToken)
    {
        var includeQuery = Query
                .Include(i => i.Inventory);

        if (request.UserId.HasValue && !string.IsNullOrWhiteSpace(request.Subject)
           && !string.IsNullOrWhiteSpace(request.Intent))
        {
            return await includeQuery
                .Where(i => i.Inventory!.UserId == request.UserId && i.Inventory.Subject == request.Subject && i.Intent == request.Intent)
                .Select(i => i.Version)
                .MaxAsync(cancellationToken);
        }

        return 0;
    }
    public Task<Models.InventoryHistory> Save(SaveCommand request, CancellationToken cancellationToken)
    {
        return base.Save(request, cancellationToken);
    }

}
