using Inventory.Contracts;

namespace Inventory.Features.Function;

public interface IFunctionRepository : IRepository<IInventoryDbContext, Models.Function>, IEncryptor<Models.Function>, IDecryptor<Models.Function>
{
    Task<Models.Function?> Get(GetRequest request, CancellationToken cancellationToken);
    Task<Models.Function> Save(SaveCommand command, CancellationToken cancellationToken);
}
