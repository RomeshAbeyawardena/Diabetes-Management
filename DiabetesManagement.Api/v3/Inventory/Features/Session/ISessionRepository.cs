using Inventory.Contracts;

namespace Inventory.Features.Session
{
    public interface ISessionRepository : IRepository<IInventoryDbContext, Models.Session>
    {
        Task<Models.Session?> Get(GetRequest request, CancellationToken cancellationToken);
        Task<Models.Session> Save(SaveCommand command, CancellationToken cancellationToken);
    }
}
