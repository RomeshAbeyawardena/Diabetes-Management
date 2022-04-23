using Inventory.Contracts;

namespace Inventory.Features.Application;

public interface IApplicationRepository : IRepository<IInventoryDbContext, Models.Application>, IEncrypt<Models.Application>, IDecrypt<Models.Application>
{
    Task<Models.Application> Save(SaveCommand saveCommand, CancellationToken cancellationToken);
}
