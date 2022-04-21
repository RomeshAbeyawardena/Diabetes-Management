using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;
using DiabetesManagement.Features.AccessToken;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace DiabetesManagement.Features;

public class AuthenticationRequestHandler<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : class
{
    private readonly IHttpContextAccessor httpContext;
    private readonly IMediator mediator;
    private readonly IJwtProvider jwtProvider;
    private readonly ApplicationSettings applicationSettings;
    private readonly TokenValidationParameters validationParameters;

    private async Task<IEnumerable<string?>> GetClaims(string accessTokenKey, string accessTokenValue)
    {
        if(accessTokenKey == "sys.admin" && accessTokenValue.Equals(applicationSettings.SystemAdministratorUser))
        {

        }

        if (Guid.TryParse(accessTokenKey, out var key))
        {
            var accessToken = await mediator.Send(new GetRequest { Key = key, AccessToken = accessTokenValue });

            if (accessToken != null && accessToken.AccessTokenClaims != null)
            {
                return accessToken.AccessTokenClaims.Select(a => a.Claim);
            }
        }

        throw new UnauthorizedAccessException();
    }

    public AuthenticationRequestHandler(IHttpContextAccessor httpContext, IMediator mediator, IJwtProvider jwtProvider, ApplicationSettings applicationSettings)
    {
        this.httpContext = httpContext;
        this.mediator = mediator;
        this.jwtProvider = jwtProvider;
        this.applicationSettings = applicationSettings;
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

    public IHttpContextAccessor HttpContext => httpContext;

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if(httpContext.HttpContext == null)
        {
            throw new NullReferenceException();
        }

        var context = httpContext.HttpContext;

        if(context.Request.Headers.TryGetValue("x-api-acc-token", out var accessToken))
        {

            var parameters = jwtProvider.Extract(accessToken, validationParameters);
            IEnumerable<string> claims = Array.Empty<string>();
            if (parameters != null && parameters.TryGetValue("api-key", out var apiKey)
                && parameters.TryGetValue("api-value", out var value))
            {
                claims = await GetClaims(apiKey, value);
            }
            var requestType = request.GetType();
            var claimsAttribute = requestType.GetCustomAttribute<RequiresClaimsAttribute>();

            if(claimsAttribute == null || !claimsAttribute.Claims.Any())
            {
                //if the request does not have any claims it is assumed accessible publicly 
                return;
            }
            else if(claims.Any(c => claimsAttribute.Claims.Contains(c)))
            {
                //consists of matching claims grant access
                return;
            }
        }

        //no access granted either the http context isn't properly authorised or the session does not match the specified claim
        throw new UnauthorizedAccessException();
    }
}
