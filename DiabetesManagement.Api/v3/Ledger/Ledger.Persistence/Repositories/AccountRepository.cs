using Inventory;
using Inventory.Contracts;
using Inventory.Extensions;
using Ledger.Features.Account;
using Ledger.Persistence.Base;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Persistence.Repositories;

public class AccountRepository : LedgerRepositoryBase<Models.Account>, IAccountRepository, IEncryptor<Models.Account>, IDecryptor<Models.Account>
{
    private readonly IClockProvider clockProvider;
    private readonly ApplicationSettings applicationSettings;

    protected override Task<bool> Add(Models.Account model, CancellationToken cancellationToken)
    {
        model.Created = clockProvider.Clock.UtcNow;
        model.Enabled = true;
        return AcceptChanges;
    }

    protected override Task<bool> Update(Models.Account model, CancellationToken cancellationToken)
    {
        model.Modified = clockProvider.Clock.UtcNow;
        return AcceptChanges;
    }

    protected override Task<bool> Validate(EntityState entityState, Models.Account model, CancellationToken cancellationToken)
    {
        Encrypt(model);
        return entityState == EntityState.Added 
            ? Query.AnyAsync(a => a.Reference == model.Reference, cancellationToken)
            : Query.AnyAsync(a => a.Id != model.Id && a.Reference == model.Reference, cancellationToken);
    }

    public AccountRepository(IDbContextProvider dbContextProvider, IClockProvider clockProvider, ApplicationSettings applicationSettings) : base(dbContextProvider)
    {
        this.clockProvider = clockProvider;
        this.applicationSettings = applicationSettings;
    }

    public Task<Models.Account?> Get(GetRequest request, CancellationToken cancellationToken)
    {
        if (request.AccountId.HasValue) 
        {
            return FindAsync(s => s.Id == request.AccountId, cancellationToken);
        }

        return FindAsync(s => s.Reference == request.Reference, cancellationToken);
    }

    public Task<Models.Account> SaveAccount(SaveCommand command, CancellationToken cancellationToken)
    {
        return base.Save(command, cancellationToken);
    }

    public void Encrypt(Models.Account model)
    {
        if(model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (!string.IsNullOrWhiteSpace(model.Reference))
        {
            model.Reference = model.Reference.Encrypt(applicationSettings.Algorithm, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out var caseSignature);

            model.ReferenceCaseSignature = caseSignature;
        }
    }

    public void Decrypt(Models.Account model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (!string.IsNullOrWhiteSpace(model.Reference) && !string.IsNullOrWhiteSpace(model.ReferenceCaseSignature))
        {
            model.Reference = model.Reference.Decrypt(applicationSettings.Algorithm, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, model.ReferenceCaseSignature);
        }
    }
}
