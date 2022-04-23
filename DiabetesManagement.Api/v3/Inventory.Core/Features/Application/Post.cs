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
    private readonly IMediator mediator;
    
    public Post(IClockProvider clockProvider, IApplicationRepository applicationRepository,
        IMediator mediator)
    {
        this.clockProvider = clockProvider;
        this.applicationRepository = applicationRepository;
        this.mediator = mediator;
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
            await mediator.Send(new AccessTokenFeature.PostCommand
            {
                Claims = claims,
                Key = request.Intent,
                Value = request.AccessToken,
                Expires = request.Expires.HasValue
                    ? clockProvider.Clock.UtcNow.Add(request.Expires.Value)
                    : null,
                ApplicationId = application.ApplicationId,
            }, cancellationToken);
        }
        applicationRepository.Decrypt(application);

        return application;
    }
}
