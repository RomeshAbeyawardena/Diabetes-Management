using DiabetesManagement.Contracts;
using DiabetesManagement.Core.Base;
using DiabetesManagement.Features.Session;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Core.Features.Session
{
    public class SessionRepository : InventoryDbRepositoryBase<Models.Session>, ISessionRepository
    {
        private readonly IClockProvider clockProvider;
        private readonly ApplicationSettings applicationSettings;
        private bool expireSession = false;
        protected override Task<bool> Add(Models.Session session, CancellationToken cancellationToken)
        {
            var currentDate = DateTimeOffset.UtcNow;
            session.SessionId = Guid.NewGuid();
            session.Created = currentDate;
            session.Expires = session.Created.Add(applicationSettings.SessionExpiry);
            return Task.FromResult(true);
        }

        protected override Task<bool> Update(Models.Session session, CancellationToken cancellationToken)
        {
            var currentDate = clockProvider.Clock.UtcNow;
            if (session.Expires.HasValue && session.Expires > currentDate)
            {
                if (expireSession)
                {
                    session.Expires = session.Expires.Value.Subtract(applicationSettings.SessionExpiry);
                    session.Enabled = false;
                }
                else
                {
                    var diff = session.Expires.Value
                            .Subtract(currentDate);

                    var diff2 = applicationSettings.SessionExpiry
                            .Subtract(diff);

                    session.Expires = session.Expires.Value
                        .Add(diff2);
                }
            }

            return Task.FromResult(true);
        }

        public SessionRepository(IDbContextProvider context, IClockProvider clockProvider, ApplicationSettings applicationSettings) : base(context)
        {
            this.clockProvider = clockProvider;
            this.applicationSettings = applicationSettings;
        }

        public async Task<Models.Session?> Get(GetRequest request, CancellationToken cancellationToken)
        {
            if(request.AuthenticateSession)
            {
                return await Query.Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.SessionId == request.SessionId && s.UserId == request.UserId && s.Enabled
                    && s.Expires >= DateTimeOffset.UtcNow, cancellationToken);
            }

            if (request.UserId.HasValue)
            {
                return await Query.FirstOrDefaultAsync(s => s.UserId == request.UserId && s.Enabled
                    && s.Expires >= DateTimeOffset.UtcNow, cancellationToken);
            }

            if (request.SessionId.HasValue)
            {
                return await FindAsync(s => s.SessionId == request.SessionId.Value, cancellationToken: cancellationToken);
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
            expireSession = command.ExpireSession;
            await Save(session, cancellationToken);

            if (command.CommitChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }

            return session;
        }
    }
}
