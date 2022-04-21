﻿using DiabetesManagement.Contracts;

namespace DiabetesManagement.Features.AccessToken;

public interface IAccessTokenRepository : IRepository<IInventoryDbContext, Models.AccessToken>
{
    Task<Models.AccessToken?> Get(string accessToken, CancellationToken cancellationToken);
    Task<Models.AccessToken> Save(SaveCommand saveCommand, CancellationToken cancellationToken);
}
