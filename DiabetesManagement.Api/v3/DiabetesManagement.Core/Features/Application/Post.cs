using DiabetesManagement.Contracts;
using AccessTokenFeature = DiabetesManagement.Features.AccessToken;
using DiabetesManagement.Features.Application;
using MediatR;

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
        var hasClaims = request.Claims != null && !request.Claims.Any();

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
            await accessTokenRepository.Save(new Models.AccessToken
            {
                Value = request.AccessToken,
                Expires = request.Expires.HasValue
                    ? clockProvider.Clock.UtcNow.Add(request.Expires.Value)
                    : null,
                ApplicationId = application.ApplicationId
            }, cancellationToken);
        }

        return application;
    }
}
