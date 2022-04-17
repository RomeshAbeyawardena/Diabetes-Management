using DiabetesManagement.Contracts;
using DiabetesManagement.Core.Base;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.Inventory;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Core.Features.Inventory;

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

    public InventoryRepository(IDbContextProvider context, IClockProvider clockProvider) : base(context)
    {
        this.clockProvider = clockProvider;
    }

    public async Task<IEnumerable<Models.Inventory>> Get(GetRequest request, CancellationToken cancellationToken)
    {
        return await Query
            .Where(i => i.UserId == request.UserId && i.Subject == request.Key && i.DefaultIntent == request.Type)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Models.Inventory> Save(SaveCommand postRequest, CancellationToken cancellationToken)
    {
        return await base.Save(postRequest, cancellationToken);
    }
}
