using Inventory.Contracts;
using Inventory.Features.AccessToken;
using MediatR;

namespace Inventory.Core.Features.AccessToken;

public class Sign : IRequestHandler<SignRequest, string>
{
    private readonly IJwtProvider jwtProvider;
    private readonly IMediator mediator;

    public Sign(IJwtProvider jwtProvider, IMediator mediator)
    {
        this.jwtProvider = jwtProvider;
        this.mediator = mediator;
    }

    public async Task<string> Handle(SignRequest request, CancellationToken cancellationToken)
    {
        var jwtdict = new Dictionary<string, object> {
            { Keys.ApiToken, request.ApiKey! },
            { Keys.ApiIntent, request.ApiIntent! },
            { Keys.ApiTokenChallenge, request.ApiChallenge! } };

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
                jwtdict.Add(Keys.ApplicationId, accessToken.ApplicationId);
            }
        }

        return jwtProvider.BuildToken(jwtdict, jwtProvider.DefaultTokenValidationParameters);
    }
}
