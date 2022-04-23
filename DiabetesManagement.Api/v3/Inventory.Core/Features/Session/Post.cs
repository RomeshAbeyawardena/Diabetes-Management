using AutoMapper;
using Inventory.Contracts;
using Inventory.Features.Session;
using MediatR;

namespace Inventory.Core.Features.Session;

public class Post : IRequestHandler<PostCommand, Models.Session>
{
    private readonly IMapper mapper;
    private readonly ISessionRepository sessionRepository;
    private readonly IDecrypt<Models.User> decryptor;

    public Post(IMapper mapper, ISessionRepository sessionRepository, IDecrypt<Models.User> decryptor)
    {
        this.mapper = mapper;
        this.sessionRepository = sessionRepository;
        this.decryptor = decryptor;
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

        session = await sessionRepository.Save(new SaveCommand
        {
            ExpireSession = request.ExpireSession,
            Session = session,
            CommitChanges = true
        }, cancellationToken);


        if (session.User != null)
        {
            decryptor.Decrypt(session.User);
        }

        return session;
    }
}
