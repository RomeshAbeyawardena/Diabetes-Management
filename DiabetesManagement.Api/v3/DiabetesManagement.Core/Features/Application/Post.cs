using DiabetesManagement.Contracts;
using AccessTokenFeature = DiabetesManagement.Features.AccessToken;
using DiabetesManagement.Features.Application;
using MediatR;
using System.Collections.ObjectModel;

namespace DiabetesManagement.Core.Features.Application;

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
        var hasClaims = request.Claims != null && request.Claims.Any();

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
                UserId = request.UserId ?? throw new NullReferenceException()
            },
            CommitChanges = request.Claims == null || !request.Claims.Any()
        }, cancellationToken);

        if (hasClaims)
        {
            var accessToken = await accessTokenRepository.Save(new Models.AccessToken
            {
                Value = request.AccessToken,
                Expires = request.Expires.HasValue
                    ? clockProvider.Clock.UtcNow.Add(request.Expires.Value)
                    : null,
                ApplicationId = application.ApplicationId
            }, cancellationToken);

            accessToken.Entity.AccessTokenClaims ??= new Collection<Models.AccessTokenClaim>();
            
            foreach (var claim in request.Claims!)
            {
                accessToken.Entity.AccessTokenClaims.Add(new Models.AccessTokenClaim { 
                    Claim = claim, 
                    Created = clockProvider.Clock.UtcNow, 
                    Expires = clockProvider.Clock.UtcNow.AddDays(365) 
                });
            }

        }

        return application;
    }
}
