using Inventory.Contracts;

namespace Inventory.Features.ApplicationInstance
{
    public interface IApplicationInstanceRepository : IRepository<IInventoryDbContext, Models.ApplicationInstance>
    {
        Task<Models.ApplicationInstance> Save(SaveCommand saveCommand, CancellationToken cancellationToken);
    }
}
