using AutoMapper;
using Inventory.Features.ApplicationInstance;
using MediatR;

namespace Inventory.Core.Features.ApplicationInstance;

public class Put : IRequestHandler<PutCommand, Models.ApplicationInstance>
{
    private readonly IMapper mapper;
    private readonly IApplicationInstanceRepository applicationInstanceRepository;

    public Put(IMapper mapper, IApplicationInstanceRepository applicationInstanceRepository)
    {
        this.mapper = mapper;
        this.applicationInstanceRepository = applicationInstanceRepository;
    }

    public Task<Models.ApplicationInstance> Handle(PutCommand request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<SaveCommand>(request);
        return applicationInstanceRepository.Save(command, cancellationToken);
    }
}
