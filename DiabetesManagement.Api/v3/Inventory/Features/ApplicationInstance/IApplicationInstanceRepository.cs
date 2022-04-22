using Inventory.Contracts;

namespace Inventory.Features.ApplicationInstance;

public interface IApplicationInstanceRepository : IRepository<IInventoryDbContext, Models.ApplicationInstance>
{
    Task<IEnumerable<Models.ApplicationInstance>> Get(GetRequest request, CancellationToken cancellationToken);
    Task<Models.ApplicationInstance> Save(SaveCommand saveCommand, CancellationToken cancellationToken);
}
