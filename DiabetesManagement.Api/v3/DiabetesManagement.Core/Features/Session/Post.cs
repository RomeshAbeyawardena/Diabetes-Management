using DiabetesManagement.Features.Session;
using MediatR;

namespace DiabetesManagement.Core.Features.Session;

public class Post : IRequestHandler<PostCommand, Models.Session>
{
    private readonly ISessionRepository sessionRepository;

    public Post(ISessionRepository sessionRepository)
    {
        this.sessionRepository = sessionRepository;
    }

    public async Task<Models.Session> Handle(PostCommand request, CancellationToken cancellationToken)
    {
        var session = await sessionRepository.Get(new GetRequest { SessionId = request.SessionId, UserId = request.UserId }, cancellationToken);

        if(session == null)
        {
            session = new Models.Session
            {
                SessionId = request.SessionId.HasValue ? request.SessionId.Value : default,
                UserId = request.UserId!.Value,
                Enabled = true
            };
        }

        return await sessionRepository.Save(new SaveCommand
        {
            Session = session,
            CommitChanges = true
        }, cancellationToken);
    }
}
