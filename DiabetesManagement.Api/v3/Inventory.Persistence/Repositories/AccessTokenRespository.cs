using Inventory.Contracts;
using Inventory.Extensions;
using Inventory.Features.AccessToken;
using Inventory.Persistence.Base;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Repositories;

public class AccessTokenRespository : InventoryDbRepositoryBase<Models.AccessToken>, IAccessTokenRepository
{
    private readonly IClockProvider clockProvider;
    private readonly ApplicationSettings applicationSettings;
    private bool prepareEncryptedFields;
    protected override Task<bool> Add(Models.AccessToken entity, CancellationToken cancellationToken)
    {
        entity.Created = clockProvider.Clock.UtcNow;
        entity.Enabled = true;
        return AcceptChanges;
    }

    protected override async Task<bool> Validate(EntityState entityState, Models.AccessToken model, CancellationToken cancellationToken)
    {
        if(entityState == EntityState.Added || prepareEncryptedFields)
        {
            Encrypt(model);
        }

        return await (entityState == EntityState.Added
            ? Query.AnyAsync(i => i.ApplicationId == model.ApplicationId && i.Key == model.Key, cancellationToken)
            : Query.AnyAsync(i => i.AccessTokenId != model.AccessTokenId 
                && i.ApplicationId == model.ApplicationId && i.Key == model.Key, cancellationToken)) == false;
    }

    public AccessTokenRespository(IDbContextProvider dbContextProvider, 
        IClockProvider clockProvider, ApplicationSettings applicationSettings) : base(dbContextProvider)
    {
        this.clockProvider = clockProvider;
        this.applicationSettings = applicationSettings;
    }

    public Task<Models.AccessToken?> Get(GetRequest request, CancellationToken cancellationToken)
    {
        return Query.Include(a => a.AccessTokenClaims).FirstOrDefaultAsync(a => a.AccessTokenId == request.Key
            && a.Key == request.Intent
            && a.Value == request.AccessToken, cancellationToken);
    }

    public Task<Models.AccessToken> Save(SaveCommand saveCommand, CancellationToken cancellationToken)
    {
        prepareEncryptedFields = saveCommand.PrepareEncryptedFields;
        return base.Save(saveCommand, cancellationToken);
    }

    public void Encrypt(Models.AccessToken model)
    {
        if (!string.IsNullOrWhiteSpace(model.Key))
        {
            model.Key = model.Key.Encrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes,
                applicationSettings.ServerInitialVectorBytes, out var caseSignature);
            model.KeySignature = caseSignature;
        }
    }

    public void Decrypt(Models.AccessToken model)
    {
        if (!string.IsNullOrWhiteSpace(model.Key))
        {
            model.Key.Decrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes,
                 applicationSettings.ServerInitialVectorBytes, model.KeySignature);
        }
    }
}
