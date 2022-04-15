using DiabetesManagement.Core.Base;
using DiabetesManagement.Features.Application;

namespace DiabetesManagement.Core.Features.Application;

public class ApplicationRepository : InventoryDbRepositoryBase<Models.Application>, IApplicationRepository
{
    private void PrepareEncryptedFields()
    {

    }

    public ApplicationRepository(InventoryDbContext context) : base(context)
    {
    }

    public Task<Models.Application> Save(SaveCommand saveCommand, CancellationToken cancellationToken)
    {
        if(saveCommand.Application == null)
        {
            throw new NullReferenceException();
        }

        var application = saveCommand.Application;

        if(application.ApplicationId != default)
        {
            
        }
        else
        {
            
        }
    }
}
