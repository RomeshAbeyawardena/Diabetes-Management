﻿using DiabetesManagement.Core.Base;
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
                return await DbSet.Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.SessionId == request.SessionId && s.UserId == request.UserId && s.Enabled
                    && s.Expires >= DateTimeOffset.UtcNow, cancellationToken);
            }

            if (request.UserId.HasValue)
            {
                return await DbSet.FirstOrDefaultAsync(s => s.UserId == request.UserId && s.Enabled
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
            var currentDate = DateTimeOffset.UtcNow;
            if (session.SessionId == default)
            {
                session.SessionId = Guid.NewGuid();
                session.Created = currentDate;
                session.Expires = session.Created.Add(applicationSettings.SessionExpiry);
                Add(session);
            }
            else
            {
                if (session.Expires.HasValue && session.Expires > currentDate)
                {
                    if (command.ExpireSession)
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

                Update(session);
            }

            if (command.CommitChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }

            return session;
        }
    }
}
