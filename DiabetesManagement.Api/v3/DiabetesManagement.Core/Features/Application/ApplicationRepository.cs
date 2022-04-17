using DiabetesManagement.Contracts;
using DiabetesManagement.Core.Base;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.Application;
using Microsoft.EntityFrameworkCore;

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

    protected override Task<bool> IsMatch(Models.Application application, CancellationToken cancellationToken)
    {
        return Query.AnyAsync(a => a.ApplicationId == application.ApplicationId 
            && a.Hash == application.Hash, cancellationToken);
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

    public ApplicationRepository(IDbContextProvider dbContextProvider, ApplicationSettings applicationSettings, IClockProvider clockProvider) : base(dbContextProvider)
    {
        this.applicationSettings = applicationSettings;
        this.clockProvider = clockProvider;
    }

    public async Task<Models.Application> Save(SaveCommand saveCommand, CancellationToken cancellationToken)
    {
        return await base.Save(saveCommand, cancellationToken);
    }
}
