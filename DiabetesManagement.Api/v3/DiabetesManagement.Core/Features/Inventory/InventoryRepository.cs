using DiabetesManagement.Contracts;
using DiabetesManagement.Core.Base;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.Inventory;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Core.Features.Inventory;

public class InventoryRepository : InventoryDbRepositoryBase<Models.Inventory>, IInventoryRepository
{
    private readonly IClockProvider clockProvider;

    public InventoryRepository(IDbContextProvider context, IClockProvider clockProvider) : base(context)
    {
        this.clockProvider = clockProvider;
    }

    public async Task<IEnumerable<Models.Inventory>> Get(GetRequest request, CancellationToken cancellationToken)
    {
        return await DbSet
            .Where(i => i.UserId == request.UserId && i.Subject == request.Key && i.DefaultIntent == request.Type)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Models.Inventory> Save(SaveCommand postRequest, CancellationToken cancellationToken)
    {
        if(postRequest.Inventory == null)
        {
            throw new NullReferenceException();
        }

        var inventory = postRequest.Inventory;
        var currentDate = clockProvider.Clock.UtcNow;
        if (inventory.InventoryId == default)
        {
            inventory.InventoryId = Guid.NewGuid();
            inventory.Created = currentDate;
            inventory.Hash = inventory.GetHash();
            Add(inventory);
        }
        else
        {
            inventory.Modified = currentDate;
            Update(inventory);
        }

        if (postRequest.CommitChanges)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }

        return inventory;
    }
}
