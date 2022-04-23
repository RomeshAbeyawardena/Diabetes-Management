using Inventory.Contracts;

namespace Inventory.Features.User
{
    public interface IUserRepository : IRepository<IInventoryDbContext, Models.User>, IEncrypt<Models.User>, IDecrypt<Models.User>
    {
        Task<Models.User?> GetUser(GetRequest request, CancellationToken cancellationToken);
        Task<Models.User> SaveUser(SaveCommand command, CancellationToken cancellationToken);
    }
}
