using Inventory.Extensions;
using Inventory.Contracts;
using Inventory.Features.Application;
using Inventory.Persistence.Base;
using Microsoft.EntityFrameworkCore;
using MediatR;
using UserFeature = Inventory.Features.User;

namespace Inventory.Persistence.Repositories;

public class ApplicationRepository : InventoryDbRepositoryBase<Models.Application>, IApplicationRepository
{
    private readonly ApplicationSettings applicationSettings;
    private readonly IClockProvider clockProvider;
    private readonly IMediator mediator;
    private bool prepareEncryptedFields = false;

    protected override async Task<bool> Validate(EntityState entityState, Models.Application model, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new UserFeature.GetRequest { UserId = model.UserId }, cancellationToken);

        if(user == null)
        {
            return false;
        }

        if (entityState == EntityState.Added || prepareEncryptedFields)
        {
            Encrypt(model);
        }

        return await (entityState == EntityState.Added 
            ? Query.AnyAsync(a => a.Name == model.Name && a.UserId == model.UserId, cancellationToken)
            : Query.AnyAsync(a => a.ApplicationId != model.ApplicationId 
                && a.Name == model.Name && a.UserId == model.UserId, cancellationToken)) == false;
    }

    protected override Task<bool> IsMatch(Models.Application application, CancellationToken cancellationToken)
    {
        return Query.AnyAsync(a => a.ApplicationId == application.ApplicationId
            && a.Hash == application.Hash, cancellationToken);
    }

    protected override Task<bool> Add(Models.Application application, CancellationToken cancellationToken)
    {
        var currentDate = clockProvider.Clock.UtcNow;
        
        application.Created = currentDate;
        application.Enabled = true;

        if (!application.Expires.HasValue && applicationSettings.DefaultApplicationExpiry.HasValue)
        {
            application.Expires = currentDate.Add(applicationSettings.DefaultApplicationExpiry.Value);
        }

        application.Hash = application.GetHash();
        return AcceptChanges;
    }

    protected override async Task<bool> Update(Models.Application application, CancellationToken cancellationToken)
    {
        if (await IsMatch(application, cancellationToken))
        {
            application.Modified = clockProvider.Clock.UtcNow;
            application.Hash = application.GetHash();
            return true;
        }

        return false;
    }

    public ApplicationRepository(IDbContextProvider dbContextProvider, ApplicationSettings applicationSettings, 
        IClockProvider clockProvider, IMediator mediator) : base(dbContextProvider)
    {
        this.applicationSettings = applicationSettings;
        this.clockProvider = clockProvider;
        this.mediator = mediator;
    }

    public async Task<Models.Application> Save(SaveCommand saveCommand, CancellationToken cancellationToken)
    {
        prepareEncryptedFields = saveCommand.PrepareEncryptedFields;
        return await base.Save(saveCommand, cancellationToken);
    }

    public void Encrypt(Models.Application application)
    {
        application.DisplayName = application.DisplayName!.Encrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out string caseSignature);
        application.DisplayNameCaseSignature = caseSignature;

        application.Name = application.Name!.Encrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out caseSignature);
        application.NameCaseSignature = caseSignature;
    }

    public void Decrypt(Models.Application application)
    {
        application.DisplayName = application.DisplayName!.Decrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, application.DisplayNameCaseSignature);

        application.Name = application.Name!.Decrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, application.NameCaseSignature);
    }
}
