using DiabetesManagement.Contracts;
using DiabetesManagement.Core.Base;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.Application;

namespace DiabetesManagement.Core.Features.Application;

public class ApplicationRepository : InventoryDbRepositoryBase<Models.Application>, IApplicationRepository
{
    private readonly ApplicationSettings applicationSettings;
    private readonly IClockProvider clockProvider;

    private void PrepareEncryptedFields(Models.Application application)
    {
        application.DisplayName = application.DisplayName!.Encrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out string caseSignature);
        application.DisplayNameCaseSignature = caseSignature;
        application.Name = application.Name!.Encrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out caseSignature);
        application.DisplayNameCaseSignature = caseSignature;
    }

    protected override Task<bool> Add(Models.Application application, CancellationToken cancellationToken)
    {
        var currentDate = clockProvider.Clock.UtcNow;
        PrepareEncryptedFields(application);
        application.Created = currentDate;
        application.Enabled = true;

        if (!application.Expires.HasValue && applicationSettings.DefaultApplicationExpiry.HasValue)
        {
            application.Expires = currentDate.Add(applicationSettings.DefaultApplicationExpiry.Value);
        }

        application.Hash = application.GetHash();
        Add(application);
        return Task.FromResult(true);
    }

    protected override Task<bool> Update(Models.Application application, CancellationToken cancellationToken)
    {
        application.Hash = application.GetHash();
        return Task.FromResult(true);
    }

    public ApplicationRepository(IDbContextProvider dbContextProvider, ApplicationSettings applicationSettings, IClockProvider clockProvider) : base(dbContextProvider)
    {
        this.applicationSettings = applicationSettings;
        this.clockProvider = clockProvider;
    }

    public async Task<Models.Application> Save(SaveCommand saveCommand, CancellationToken cancellationToken)
    {
        if(saveCommand.Application == null)
        {
            throw new NullReferenceException();
        }

        var application = saveCommand.Application;

        await Save(application, cancellationToken);

        if (saveCommand.CommitChanges)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }

        return application!;
    }
}
