using AccessTokenFeature = Inventory.Features.AccessToken;
using Inventory.Features.Application;
using MediatR;
using System.Collections.ObjectModel;
using Inventory.Contracts;

namespace Inventory.Core.Features.Application;

public class Post : IRequestHandler<PostCommand, Models.Application>
{
    private readonly IClockProvider clockProvider;
    private readonly IApplicationRepository applicationRepository;
    private readonly AccessTokenFeature.IAccessTokenRepository accessTokenRepository;

    public Post(IClockProvider clockProvider, IApplicationRepository applicationRepository,
        AccessTokenFeature.IAccessTokenRepository accessTokenRepository)
    {
        this.clockProvider = clockProvider;
        this.applicationRepository = applicationRepository;
        this.accessTokenRepository = accessTokenRepository;
    }

    public async Task<Models.Application> Handle(PostCommand request, CancellationToken cancellationToken)
    {
        var claims = request.Claims!.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var hasClaims = claims.Any();

        var application = await applicationRepository.Save(new SaveCommand
        {
            Application = new Models.Application
            {
                DisplayName = request.DisplayName,
                Expires = request.Expires.HasValue
                    ? clockProvider.Clock.UtcNow.Add(request.Expires.Value)
                    : null,
                Name = request.Name,
                Intent = request.Intent,
                UserId = request.UserId ?? throw new NullReferenceException("User not specified")
            },
            CommitChanges = !claims.Any()
        }, cancellationToken);

        if (hasClaims)
        {
            var accessToken = new Models.AccessToken
            {
                Key = request.Intent,
                Value = request.AccessToken,
                Expires = request.Expires.HasValue
                    ? clockProvider.Clock.UtcNow.Add(request.Expires.Value)
                    : null,
                ApplicationId = application.ApplicationId,
            };

            accessToken.AccessTokenClaims ??= new Collection<Models.AccessTokenClaim>();

            foreach (var claim in claims!)
            {
                accessToken.AccessTokenClaims.Add(new Models.AccessTokenClaim
                {
                    Claim = claim,
                    Created = clockProvider.Clock.UtcNow,
                    Expires = clockProvider.Clock.UtcNow.AddDays(365)
                });
            }

            await accessTokenRepository.Save(new AccessTokenFeature.SaveCommand
            {
                AccessToken = accessToken,
                CommitChanges = true
            }, cancellationToken);

        }
        applicationRepository.Decrypt(application);

        return application;
    }
}
