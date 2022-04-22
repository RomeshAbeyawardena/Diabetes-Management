using Inventory.Features.Session;
using MediatR;

namespace Inventory.Core.Features.Session;

public class Get : IRequestHandler<GetRequest, Models.Session>
{
    private readonly ISessionRepository sessionRepository;

    public Get(ISessionRepository sessionRepository)
    {
        this.sessionRepository = sessionRepository;
    }

    public Task<Models.Session> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        return sessionRepository.Get(request, cancellationToken)!;
    }
}
