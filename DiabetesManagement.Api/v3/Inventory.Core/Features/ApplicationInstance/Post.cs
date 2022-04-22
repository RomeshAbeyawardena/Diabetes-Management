using Inventory.Features.ApplicationInstance;
using MediatR;

namespace Inventory.Core.Features.ApplicationInstance;

public class Post : IRequestHandler<PostCommand, Models.ApplicationInstance>
{
    private readonly IApplicationInstanceRepository applicationInstanceRepository;

    public Post(IApplicationInstanceRepository applicationInstanceRepository)
    {
        this.applicationInstanceRepository = applicationInstanceRepository;
    }

    public Task<Models.ApplicationInstance> Handle(PostCommand request, CancellationToken cancellationToken)
    {
        return applicationInstanceRepository.Save(new SaveCommand
        {
            ApplicationInstance = new Models.ApplicationInstance
            {
                ApplicationId = request.ApplicationId,
                Expires = request.Expires
            },
            CommitChanges = true
        }, cancellationToken);
    }
}
