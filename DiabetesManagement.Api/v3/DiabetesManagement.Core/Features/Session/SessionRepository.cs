using DiabetesManagement.Core.Base;
using DiabetesManagement.Features.Session;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Core.Features.Session
{
    public class SessionRepository : InventoryDbRepositoryBase<Models.Session>, ISessionRepository
    {
        private readonly ApplicationSettings applicationSettings;

        public SessionRepository(InventoryDbContext context, ApplicationSettings applicationSettings) : base(context)
        {
            this.applicationSettings = applicationSettings;
        }

        public async Task<Models.Session?> Get(GetRequest request, CancellationToken cancellationToken)
        {
            if(request.AuthenticateSession)
            {
                return await DbSet.FirstOrDefaultAsync(s => s.SessionId == request.SessionId && s.UserId == request.UserId
                    && s.Expires >= DateTimeOffset.UtcNow, cancellationToken);
            }

            if (request.UserId.HasValue)
            {
                return await DbSet.FirstOrDefaultAsync(s => s.UserId == request.UserId
                    && s.Expires >= DateTimeOffset.UtcNow, cancellationToken);
            }

            if (request.SessionId.HasValue)
            {
                return await DbSet.FindAsync(new object[] { request.SessionId.Value }, cancellationToken: cancellationToken);
            }

            return null!;
        }

        public async Task<Models.Session> Save(SaveCommand command, CancellationToken cancellationToken)
        {
            var session = command.Session;

            if (session == null)
            {
                throw new NullReferenceException();
            }
            var currentDate = DateTimeOffset.UtcNow;
            if (session.SessionId == default)
            {
                session.SessionId = Guid.NewGuid();
                session.Created = currentDate;
                session.Expires = session.Created.Add(applicationSettings.SessionExpiry);
                DbSet.Add(session);
            }
            else
            {
                if (session.Expires.HasValue && session.Expires >= currentDate)
                {
                    session.Expires = session.Expires.Value.Add(session.Expires.Value.Subtract(currentDate));
                }
            }

            if (command.CommitChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }

            return session;
        }
    }
}
