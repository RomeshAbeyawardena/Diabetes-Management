using DiabetesManagement.Contracts;
using DiabetesManagement.Features.AccessToken;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace DiabetesManagement.Core.Features.AccessToken;

public class Sign : IRequestHandler<SignRequest, string>
{
    private readonly ApplicationSettings applicationSettings;
    private readonly IJwtProvider jwtProvider;
    private readonly TokenValidationParameters validationParameters;

    public Sign(ApplicationSettings applicationSettings, IJwtProvider jwtProvider)
    {
        this.applicationSettings = applicationSettings;
        this.jwtProvider = jwtProvider;
        var securityKey = new SymmetricSecurityKey(applicationSettings.ConfidentialServerKeyBytes);
        validationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = applicationSettings.Audience,
            ValidIssuer = applicationSettings.Issuer,
            IssuerSigningKey = securityKey,
            ValidateIssuerSigningKey = true
        };
    }

    public async Task<string> Handle(SignRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return jwtProvider.BuildToken(new Dictionary<string, object> { { request.ApiKey, request.ApiChallenge } }, validationParameters);
    }
}
