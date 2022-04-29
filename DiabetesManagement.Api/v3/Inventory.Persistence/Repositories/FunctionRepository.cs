using AutoMapper;
using Inventory.Contracts;
using Inventory.Extensions;
using Inventory.Features.Function;
using Inventory.Persistence.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Inventory.Persistence.Repositories;

public class FunctionRepository : InventoryDbRepositoryBase<Models.Function>, IFunctionRepository
{
    private readonly IClockProvider clockProvider;
    private readonly IMapper mapper;
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

    public FunctionRepository(IDbContextProvider dbContextProvider, IClockProvider clockProvider, 
        IMapper mapper, ApplicationSettings applicationSettings) : base(dbContextProvider)
    {
        this.clockProvider = clockProvider;
        this.mapper = mapper;
        this.applicationSettings = applicationSettings;
    }

    public async Task<IEnumerable<Models.Function?>> Get(ListRequest request, CancellationToken cancellationToken)
    {
        var requestFunction = mapper.Map<Models.Function>(request);

        Encrypt(requestFunction);

        var query = Expression;

        if (!request.DisplayAll.HasValue || !request.DisplayAll.Value)
        {
            query = Expression.Start(f => f.Enabled);
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            Expression<Func<Models.Function, bool>> exp = f => f.Name == request.Name;
            query = Expression.IsStarted 
                ? query.And(exp) 
                : query.Start(exp);
        }

        if (!string.IsNullOrWhiteSpace(request.Path))
        {
            Expression<Func<Models.Function, bool>> exp = f => f.Path == request.Path;
            query = Expression.IsStarted
                ? query.And(exp)
                : query.Start(exp);
        }

        return await Query.Where(query).ToArrayAsync(cancellationToken);
    }

    public Task<Models.Function?> Get(GetRequest request, CancellationToken cancellationToken)
    {
        var requestFunction = mapper.Map<Models.Function>(request);

        Encrypt(requestFunction);
        return Query.FirstOrDefaultAsync(f => f.Name == requestFunction.Name 
            && f.Enabled 
            && f.Path == requestFunction.Path, cancellationToken);
    }

    public Task<Models.Function> Save(SaveCommand command, CancellationToken cancellationToken)
    {
        return base.Save(command, cancellationToken);
    }

    public void Encrypt(Models.Function model)
    {
        if(model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (!string.IsNullOrWhiteSpace(model.Name))
        {
            model.Name = model.Name.Encrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes, 
                applicationSettings.ServerInitialVectorBytes, out var caseSignature);
            model.NameCaseSignature = caseSignature;
        }

        if (!string.IsNullOrWhiteSpace(model.Path))
        {
            model.Path = model.Path.Encrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes,
                applicationSettings.ServerInitialVectorBytes, out var caseSignature);
            model.PathCaseSignature = caseSignature;
        }
    }

    public void Decrypt(Models.Function model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (!string.IsNullOrWhiteSpace(model.Name))
        {
            model.Name = model.Name.Decrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes,
                applicationSettings.ServerInitialVectorBytes, model.NameCaseSignature);
        }

        if (!string.IsNullOrWhiteSpace(model.Path))
        {
            model.Path = model.Path.Decrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes,
                applicationSettings.ServerInitialVectorBytes, model.PathCaseSignature);
        }
    }
}
