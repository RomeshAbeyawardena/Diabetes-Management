﻿using Inventory.Contracts;
using Inventory.Features.AccessToken;
using Inventory.Persistence.Base;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Repositories;

public class AccessTokenRespository : InventoryDbRepositoryBase<Models.AccessToken>, IAccessTokenRepository
{
    protected override Task<bool> Add(Models.AccessToken entity, CancellationToken cancellationToken)
    {
        entity.Enabled = true;
        return AcceptChanges;
    }

    public AccessTokenRespository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
    {
    }

    public Task<Models.AccessToken?> Get(GetRequest request, CancellationToken cancellationToken)
    {
        return Query.Include(a => a.AccessTokenClaims).FirstOrDefaultAsync(a => a.AccessTokenId == request.Key
            && a.Key == request.Intent
            && a.Value == request.AccessToken, cancellationToken);
    }

    public Task<Models.AccessToken> Save(SaveCommand saveCommand, CancellationToken cancellationToken)
    {
        return base.Save(saveCommand, cancellationToken);
    }
}
