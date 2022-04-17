using DiabetesManagement.Contracts;

namespace DiabetesManagement.Features.AccessToken;

public interface IAccessTokenRepository : IRepository<InventoryDbContext, Models.AccessToken>
{
    Task<Models.AccessToken> Save(SaveCommand saveCommand, CancellationToken cancellationToken);
}
