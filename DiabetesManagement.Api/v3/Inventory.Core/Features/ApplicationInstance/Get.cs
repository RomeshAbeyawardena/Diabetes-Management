using Inventory.Features.ApplicationInstance;
using MediatR;

namespace Inventory.Core.Features.ApplicationInstance;

public class Get : IRequestHandler<GetRequest, IEnumerable<Models.ApplicationInstance>>
{
    private readonly IApplicationInstanceRepository applicationInstanceRepository;

    public Get(IApplicationInstanceRepository applicationInstanceRepository)
    {
        this.applicationInstanceRepository = applicationInstanceRepository;
    }

    public Task<IEnumerable<Models.ApplicationInstance>> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        return applicationInstanceRepository.Get(request, cancellationToken);
    }
}
