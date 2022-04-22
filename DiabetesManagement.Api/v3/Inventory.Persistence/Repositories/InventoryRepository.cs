using Inventory.Contracts;
using Inventory.Extensions;
using Inventory.Features.Inventory;
using Inventory.Persistence.Base;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Repositories;

public class InventoryRepository : InventoryDbRepositoryBase<Models.Inventory>, IInventoryRepository
{
    private readonly IClockProvider clockProvider;

    protected override Task<bool> Add(Models.Inventory inventory, CancellationToken cancellationToken)
    {
        var currentDate = clockProvider.Clock.UtcNow;

        inventory.InventoryId = Guid.NewGuid();
        inventory.Created = currentDate;
        inventory.Hash = inventory.GetHash();

        return Task.FromResult(true);
    }

    protected override Task<bool> Update(Models.Inventory inventory, CancellationToken cancellationToken)
    {
        var currentDate = clockProvider.Clock.UtcNow;

        inventory.Modified = currentDate;
        inventory.Hash = inventory.GetHash();
        return Task.FromResult(true);
    }

    protected override async Task<bool> Validate(EntityState entityState, Models.Inventory model, CancellationToken cancellationToken)
    {
        return await (entityState == EntityState.Added
            ? Query.AnyAsync(i => i.UserId == model.UserId && i.DefaultIntent == model.DefaultIntent 
                && i.Subject == model.Subject, cancellationToken)
            : Query.AnyAsync(i => i.InventoryId != model.InventoryId && i.UserId == model.UserId 
                && i.DefaultIntent == model.DefaultIntent && i.Subject == model.Subject, cancellationToken)) == false;
    }

    public InventoryRepository(IDbContextProvider context, IClockProvider clockProvider) : base(context)
    {
        this.clockProvider = clockProvider;
    }

    public async Task<IEnumerable<Models.Inventory>> Get(GetRequest request, CancellationToken cancellationToken)
    {
        if (request.InventoryId.HasValue)
        {
            return await Query.Where(i => i.InventoryId == request.InventoryId.Value).ToArrayAsync(cancellationToken);
        }

        return await Query
            .Where(i => i.UserId == request.UserId && i.Subject == request.Subject && i.DefaultIntent == request.Intent)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Models.Inventory> Save(SaveCommand postRequest, CancellationToken cancellationToken)
    {
        return await base.Save(postRequest, cancellationToken);
    }
}
