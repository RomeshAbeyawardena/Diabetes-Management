using AutoMapper;
using Inventory.Features.Session;
using MediatR;

namespace Inventory.Core.Features.Session;

public class Post : IRequestHandler<PostCommand, Models.Session>
{
    private readonly IMapper mapper;
    private readonly ISessionRepository sessionRepository;

    public Post(IMapper mapper, ISessionRepository sessionRepository)
    {
        this.mapper = mapper;
        this.sessionRepository = sessionRepository;
    }

    public async Task<Models.Session> Handle(PostCommand request, CancellationToken cancellationToken)
    {
        var session = await sessionRepository.Get(mapper.Map<GetRequest>(request), cancellationToken);

        if (session == null)
        {
            session = new Models.Session
            {
                SessionId = request.SessionId ?? default,
                UserId = request.UserId!.Value,
                Enabled = true
            };
        }

        return await sessionRepository.Save(new SaveCommand
        {
            ExpireSession = request.ExpireSession,
            Session = session,
            CommitChanges = true
        }, cancellationToken);
    }
}
