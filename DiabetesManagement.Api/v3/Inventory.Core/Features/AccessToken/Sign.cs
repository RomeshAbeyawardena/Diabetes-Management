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
        //build dictionary for jwt token
        var jwtdict = new Dictionary<string, object> {
            { Keys.ApiToken, request.ApiKey! },
            { Keys.ApiIntent, request.ApiIntent! },
            { Keys.ApiTokenChallenge, request.ApiChallenge! } };
        
        Models.ApplicationInstance? applicationInstance = default;

        //the caller wants the jwt token validated
        if (request.Validate && Guid.TryParse(request.ApiKey, out var key))
        {
            //find the access token
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
                    //we don't have an existing access token lets create one
                    applicationInstance = await mediator.Send(new ApplicationInstanceFeature.PostCommand
                    {
                        ApplicationId = accessToken.ApplicationId,
                        //AccessToken = "AT", we are unsure what the access token is lets add it once its generated
                        Expires = clockProvider.Clock.UtcNow.Add(applicationSettings.DefaultApplicationExpiry ?? TimeSpan.FromHours(4)),
                        CommitChanges = false //nothing to be commited yet
                    }, cancellationToken);
                }

                //access token found lets add the validation id to the dictionary 
                jwtdict.Add(Keys.ApplicationId, applicationInstance.ApplicationInstanceId);
            }
        }

        var token = jwtProvider.BuildToken(jwtdict, jwtProvider.DefaultTokenValidationParameters);

        if(applicationInstance != null)
        {
            applicationInstance.AccessToken = token;
            await mediator.Send(new ApplicationInstanceFeature.PutCommand
            { 
                ApplicationInstance = applicationInstance, 
                CommitChanges = true }, cancellationToken);
        }

        return token;
    }
}
