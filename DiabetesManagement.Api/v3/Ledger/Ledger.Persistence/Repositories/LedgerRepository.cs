using AutoMapper;
using Inventory;
using Inventory.Contracts;
using Inventory.Extensions;
using Ledger.Features.Ledger;
using Ledger.Persistence.Base;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Persistence.Repositories;

public class LedgerRepository : LedgerRepositoryBase<Models.Ledger>, ILedgerRepository
{
    private readonly IClockProvider clockProvider;
    private readonly ApplicationSettings applicationSettings;
    private readonly IMapper mapper;
    private readonly IEncryptor<Models.Account> accountEncryptor;

    protected override Task<bool> Add(Models.Ledger model, CancellationToken cancellationToken)
    {
        model.Created = clockProvider.Clock.UtcNow;
        return AcceptChanges;
    }

    protected override Task<bool> Update(Models.Ledger model, CancellationToken cancellationToken)
    {
        model.Modified = clockProvider.Clock.UtcNow;
        return AcceptChanges;
    }

    protected override Task<bool> Validate(EntityState entityState, Models.Ledger model, CancellationToken cancellationToken)
    {
        Encrypt(model);
        return AcceptChanges;
    }

    public LedgerRepository(IDbContextProvider dbContextProvider, IClockProvider clockProvider, 
        ApplicationSettings applicationSettings, IMapper mapper, IEncryptor<Models.Account> accountEncryptor) : base(dbContextProvider)
    {
        this.clockProvider = clockProvider;
        this.applicationSettings = applicationSettings;
        this.mapper = mapper;
        this.accountEncryptor = accountEncryptor;
    }

    public async Task<IEnumerable<Models.Ledger>> Get(GetRequest request, CancellationToken cancellationToken)
    {
        if (request.LedgerId.HasValue)
        {
            return await Query.Where(l => l.Id == request.LedgerId).ToArrayAsync(cancellationToken);
        }

        if (request.AccountId.HasValue)
        {
            return await Query.Where(l => l.Id == request.AccountId).ToArrayAsync(cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(request.LedgerReference))
        {
            var requestLedger = mapper.Map<Models.Ledger>(request);
            Encrypt(requestLedger);
            return await Query.Where(l => l.Reference == requestLedger.Reference).ToArrayAsync(cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(request.AccountReference))
        {
            var requestLedger = mapper.Map<Models.Account>(request);
            accountEncryptor.Encrypt(requestLedger);

            return await Query.Where(l => l.Reference == requestLedger.Reference).ToArrayAsync(cancellationToken);
        }

        return Enumerable.Empty<Models.Ledger>();
    }

    public Task<Models.Ledger> SaveLedger(SaveCommand command, CancellationToken cancellationToken)
    {
        return base.Save(command, cancellationToken);
    }

    public void Encrypt(Models.Ledger model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (!string.IsNullOrWhiteSpace(model.Reference))
        {
            model.Reference = model.Reference.Encrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out var caseSignature);

            model.ReferenceCaseSignature = caseSignature;
        }
    }

    public void Decrypt(Models.Ledger model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (!string.IsNullOrWhiteSpace(model.Reference) && !string.IsNullOrWhiteSpace(model.ReferenceCaseSignature))
        {
            model.Reference = model.Reference.Decrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, model.ReferenceCaseSignature);
        }
    }
}
