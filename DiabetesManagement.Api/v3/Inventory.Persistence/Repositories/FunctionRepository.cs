using Inventory.Contracts;
using Inventory.Extensions;
using Inventory.Features.Function;
using Inventory.Persistence.Base;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Repositories;

public class FunctionRepository : InventoryDbRepositoryBase<Models.Function>, IFunctionRepository
{
    private readonly IClockProvider clockProvider;
    private readonly ApplicationSettings applicationSettings;

    protected override Task<bool> Add(Models.Function model, CancellationToken cancellationToken)
    {
        model.Enabled = true;
        model.Created = clockProvider.Clock.UtcNow;
        return AcceptChanges;
    }

    protected override Task<bool> Update(Models.Function model, CancellationToken cancellationToken)
    {
        model.Modified = clockProvider.Clock.UtcNow;
        return AcceptChanges;
    }

    protected override async Task<bool> Validate(EntityState entityState, Models.Function model, CancellationToken cancellationToken)
    {
        Encrypt(model);

        return await (entityState == EntityState.Added
            ? Query.AnyAsync(a => a.Name == model.Name && a.Path == model.Path, cancellationToken)
            : Query.AnyAsync(a => a.FunctionId != model.FunctionId && a.Name == model.Name && a.Path == model.Path, cancellationToken)) == false;
    }

    public FunctionRepository(IDbContextProvider dbContextProvider, IClockProvider clockProvider, ApplicationSettings applicationSettings) : base(dbContextProvider)
    {
        this.clockProvider = clockProvider;
        this.applicationSettings = applicationSettings;
    }

    public Task<Models.Function?> Get(GetRequest request, CancellationToken cancellationToken)
    {
        return Query.FirstOrDefaultAsync(f => f.Name == request.Name 
            && f.Enabled 
            && f.Path == request.Path, cancellationToken);
    }

    public Task<Models.Function> Save(SaveCommand command, CancellationToken cancellationToken)
    {
        return base.Save(command, cancellationToken);
    }

    public void Encrypt(Models.Function model)
    {
        if (model != null && !string.IsNullOrWhiteSpace(model.Name))
        {
            model.Name = model.Name.Encrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes, 
                applicationSettings.ServerInitialVectorBytes, out var caseSignature);
            model.NameCaseSignature = caseSignature;
        }
    }

    public void Decrypt(Models.Function model)
    {
        if (model != null && !string.IsNullOrWhiteSpace(model.Name))
        {
            model.Name = model.Name.Decrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes,
                applicationSettings.ServerInitialVectorBytes, model.NameCaseSignature);
        }
    }
}
