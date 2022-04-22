using DiabetesManagement.Contracts;

namespace DiabetesManagement.Features.User
{
    public interface IUserRepository : IRepository<IInventoryDbContext, Models.User>
    {
        Task<Models.User?> GetUser(GetRequest request, CancellationToken cancellationToken);
        Task<Models.User> SaveUser(SaveCommand command, CancellationToken cancellationToken);
    }
}
