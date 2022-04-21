using DiabetesManagement.Contracts;
using DiabetesManagement.Core.Base;
using DiabetesManagement.Features.AccessToken;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Persistence.Repositories;

public class AccessTokenRespository : InventoryDbRepositoryBase<Models.AccessToken>, IAccessTokenRepository
{
    public AccessTokenRespository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
    {
    }

    public Task<Models.AccessToken?> Get(string accessToken, CancellationToken cancellationToken)
    {
        return Query.FirstOrDefaultAsync(a => a.Value == accessToken, cancellationToken);
    }

    public Task<Models.AccessToken> Save(SaveCommand saveCommand, CancellationToken cancellationToken)
    {
        return base.Save(saveCommand, cancellationToken);
    }
}
