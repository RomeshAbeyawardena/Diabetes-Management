using Inventory.Contracts;

namespace Inventory.Features.Application;

public interface IApplicationRepository : IRepository<IInventoryDbContext, Models.Application>, IEncryptor<Models.Application>, IDecryptor<Models.Application>
{
    Task<Models.Application> Save(SaveCommand saveCommand, CancellationToken cancellationToken);
}
