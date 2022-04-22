using ApplicationInstanceFeature = Inventory.Features.ApplicationInstance;
using Inventory.Features;
using Inventory.Attributes;
using Inventory.Contracts;
using Inventory.Features.AccessToken;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace Inventory.Core.Features;

public class AuthenticationRequestHandler<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : class
{
    private readonly ApplicationSettings applicationSettings;
    private readonly IHttpContextAccessor httpContext;
    private readonly IJwtProvider jwtProvider;
    private readonly IMediator mediator;

    private async Task<IEnumerable<string?>?> GetClaims(string accessTokenKey, string accessTokenIntent, 
        string accessTokenValue, string? applicationInstanceId, CancellationToken cancellationToken)
    {
        if (accessTokenKey == Keys.SystemAdministrator && accessTokenValue.Equals(applicationSettings.SystemAdministratorUser))
        {
            return Permissions.SysAdmin;
        }

        if (!string.IsNullOrWhiteSpace(applicationInstanceId) && Guid.TryParse(applicationInstanceId, out var appInstanceId))
        {
            var applicationInstance = await mediator.Send(new ApplicationInstanceFeature.GetRequest
            {
                ApplicationInstanceId = appInstanceId
            }, cancellationToken);

            if(applicationInstance == null || !applicationInstance.Any())
            {
                throw new UnauthorizedAccessException("Unable to resolve application instance");
            }
        }

        if (Guid.TryParse(accessTokenKey, out var key))
        {
            var accessToken = await mediator.Send(new GetRequest
            {
                Key = key,
                Intent = accessTokenIntent,
                AccessToken = accessTokenValue
            }, cancellationToken);

            if (accessToken != null && accessToken.AccessTokenClaims != null)
            {
                return accessToken.AccessTokenClaims.Select(a => a.Claim);
            }
        }

        throw new UnauthorizedAccessException("Unable to authenticate api key");
    }

    public AuthenticationRequestHandler(ApplicationSettings applicationSettings, IHttpContextAccessor httpContext, IJwtProvider jwtProvider, IMediator mediator)
    {
        this.applicationSettings = applicationSettings;
        this.httpContext = httpContext;
        this.jwtProvider = jwtProvider;
        this.mediator = mediator;
    }

    public IHttpContextAccessor HttpContext => httpContext;

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (httpContext.HttpContext == null)
        {
            throw new NullReferenceException();
        }

        var context = httpContext.HttpContext;

        if (context.Request.Headers.TryGetValue("x-api-acc-token", out var accessToken))
        {
            var requestType = request.GetType();
            var claimsAttribute = requestType.GetCustomAttribute<RequiresClaimsAttribute>();
            if (claimsAttribute == null || claimsAttribute.Claims.Contains(Permissions.Anonymous_Access))
            {
                //if the request does not have any claims it is assumed accessible publicly 
                return;
            }

            var parameters = jwtProvider.Extract(accessToken, jwtProvider.DefaultTokenValidationParameters);

            IEnumerable<string?>? claims = Array.Empty<string>();
            if (parameters != null && parameters.TryGetValue(Keys.ApiToken, out var apiKey)
                && parameters.TryGetValue(Keys.ApiIntent, out var intent)
                && parameters.TryGetValue(Keys.ApiTokenChallenge, out var value))
            {
                var applicationInstanceId = parameters.ContainsKey(Keys.ApplicationId) ? parameters[Keys.ApplicationId] : null;
                claims = await GetClaims(apiKey, intent, value, applicationInstanceId, cancellationToken);
            }

            if (claims!.Any(c => claimsAttribute.Claims.Contains(c)))
            {
                //consists of matching claims grant access
                return;
            }


        }

        //no access granted either the http context isn't properly authorised or the session does not match the specified claim
        throw new UnauthorizedAccessException();
    }
}
