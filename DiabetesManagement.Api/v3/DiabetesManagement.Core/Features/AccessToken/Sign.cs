using DiabetesManagement.Contracts;
using DiabetesManagement.Features.AccessToken;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace DiabetesManagement.Core.Features.AccessToken;

public class Sign : IRequestHandler<SignRequest, string>
{
    private readonly IJwtProvider jwtProvider;
    public Sign(ApplicationSettings applicationSettings, IJwtProvider jwtProvider)
    {
        this.jwtProvider = jwtProvider;
        var securityKey = new SymmetricSecurityKey(applicationSettings.ConfidentialServerKeyBytes);
    }

    public async Task<string> Handle(SignRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return jwtProvider.BuildToken(new Dictionary<string, object> { 
            { Keys.ApiToken, request.ApiKey! }, 
            { Keys.ApiTokenChallenge, request.ApiChallenge! } }, jwtProvider.DefaultTokenValidationParameters);
    }
}
