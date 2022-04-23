using Inventory.Contracts;

namespace Inventory.Features.AccessToken;

public interface IAccessTokenRepository : IRepository<IInventoryDbContext, Models.AccessToken>, IEncryptor<Models.AccessToken>, IDecryptor<Models.AccessToken>
{
    Task<Models.AccessToken?> Get(GetRequest request, CancellationToken cancellationToken);
    Task<Models.AccessToken> Save(SaveCommand saveCommand, CancellationToken cancellationToken);
}
