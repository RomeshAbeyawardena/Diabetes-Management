using Inventory.Contracts;
using Inventory.Features.AccessToken;
using ApplicationInstanceFeature = Inventory.Features.ApplicationInstance;
using MediatR;

namespace Inventory.Core.Features.AccessToken;

public class Sign : IRequestHandler<SignRequest, string>
{
    private readonly IJwtProvider jwtProvider;
    private readonly IMediator mediator;
    private readonly IClockProvider clockProvider;
    private readonly ApplicationSettings applicationSettings;

    public Sign(IJwtProvider jwtProvider, IMediator mediator, IClockProvider clockProvider, ApplicationSettings applicationSettings)
    {
        this.jwtProvider = jwtProvider;
        this.mediator = mediator;
        this.clockProvider = clockProvider;
        this.applicationSettings = applicationSettings;
    }

    public async Task<string> Handle(SignRequest request, CancellationToken cancellationToken)
    {
        var jwtdict = new Dictionary<string, object> {
            { Keys.ApiToken, request.ApiKey! },
            { Keys.ApiIntent, request.ApiIntent! },
            { Keys.ApiTokenChallenge, request.ApiChallenge! } };
        
        Models.ApplicationInstance? applicationInstance = default;

        if (request.Validate && Guid.TryParse(request.ApiKey, out var key))
        {
            var accessToken = await mediator.Send(new GetRequest
            {
                Key = key,
                Intent = request.ApiIntent,
                AccessToken = request.ApiChallenge
            }, cancellationToken);

            if (accessToken != null)
            {
                var applicationInstances = await mediator.Send(new ApplicationInstanceFeature.GetRequest {
                    ApplicationId = accessToken.ApplicationId,
                    AccessToken = accessToken.Value
                }, cancellationToken);
                
                applicationInstance = applicationInstances.FirstOrDefault();
                
                if (applicationInstance == null)
                {
                    applicationInstance = await mediator.Send(new ApplicationInstanceFeature.PostCommand
                    {
                        ApplicationId = accessToken.ApplicationId,
                        AccessToken = "AT",
                        Expires = clockProvider.Clock.UtcNow.Add(applicationSettings.DefaultApplicationExpiry ?? TimeSpan.FromHours(4)),
                        CommitChanges = false
                    }, cancellationToken);
                }

                jwtdict.Add(Keys.ApplicationId, applicationInstance.ApplicationInstanceId);
            }
        }

        var token = jwtProvider.BuildToken(jwtdict, jwtProvider.DefaultTokenValidationParameters);

        if(applicationInstance != null)
        {

        }

        return token;
    }
}
