using DiabetesManagement.Contracts;
using DiabetesManagement.Core.Base;
using DiabetesManagement.Features.AccessToken;

namespace DiabetesManagement.Core.Features.AccessToken;

public class AccessTokenRespository : InventoryDbRepositoryBase<Models.AccessToken>, IAccessTokenRepository
{
    public AccessTokenRespository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
    {
    }

    public Task<Models.AccessToken> Save(SaveCommand saveCommand, CancellationToken cancellationToken)
    {
        return base.Save(saveCommand, cancellationToken);
    }
}
