using DiabetesManagement.Contracts;
using DiabetesManagement.Features.AccessToken;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace DiabetesManagement.Core.Features.AccessToken;

public class Sign : IRequestHandler<SignRequest, string>
{
    private readonly IJwtProvider jwtProvider;
    public Sign(IJwtProvider jwtProvider)
    {
        this.jwtProvider = jwtProvider;
    }

    public async Task<string> Handle(SignRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return jwtProvider.BuildToken(new Dictionary<string, object> { 
            { Keys.ApiToken, request.ApiKey! },
            { Keys.ApiIntent, request.ApiIntent! },
            { Keys.ApiTokenChallenge, request.ApiChallenge! } }, jwtProvider.DefaultTokenValidationParameters);
    }
}
