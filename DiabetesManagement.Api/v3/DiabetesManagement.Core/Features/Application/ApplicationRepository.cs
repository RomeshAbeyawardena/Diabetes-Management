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
        application.DisplayName = application.DisplayName.Encrypt(applicationSettings.Algorithm, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out string caseSignature);
        application.DisplayNameCaseSignature = caseSignature;
        application.Name = application.Name.Encrypt(applicationSettings.Algorithm, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out caseSignature);
        application.DisplayNameCaseSignature = caseSignature;
    }

    public ApplicationRepository(InventoryDbContext context, ApplicationSettings applicationSettings, IClockProvider clockProvider) : base(context)
    {
        this.applicationSettings = applicationSettings;
        this.clockProvider = clockProvider;
    }

    public Task<Models.Application> Save(SaveCommand saveCommand, CancellationToken cancellationToken)
    {
        if(saveCommand.Application == null)
        {
            throw new NullReferenceException();
        }

        var application = saveCommand.Application;
        var currentDate = clockProvider.Clock.UtcNow;

        if(application.ApplicationId != default)
        {
            PrepareEncryptedFields(application);
            application.Created = currentDate;
            application.Enabled = true;

            if (!application.Expires.HasValue)
            {

            }
        }
        else
        {
            
        }
    }
}
