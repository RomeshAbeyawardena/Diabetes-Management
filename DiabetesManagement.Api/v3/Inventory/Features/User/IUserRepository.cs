using Inventory.Contracts;

namespace Inventory.Features.User
{
    public interface IUserRepository : IRepository<IInventoryDbContext, Models.User>, IEncryptor<Models.User>, IDecryptor<Models.User>
    {
        Task<Models.User?> GetUser(GetRequest request, CancellationToken cancellationToken);
        Task<Models.User> SaveUser(SaveCommand command, CancellationToken cancellationToken);
    }
}
