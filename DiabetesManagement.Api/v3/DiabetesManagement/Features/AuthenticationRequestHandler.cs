using DiabetesManagement.Attributes;
using DiabetesManagement.Features.AccessToken;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace DiabetesManagement.Features;

public class AuthenticationRequestHandler<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : class
{
    private readonly IHttpContextAccessor httpContext;
    private readonly IMediator mediator;

    private async Task<IEnumerable<string?>> GetClaims(string accessTokenValue)
    {
        var accessToken = await mediator.Send(new GetRequest { AccessToken = accessTokenValue });

        if(accessToken == null || accessToken.AccessTokenClaims == null)
        {
            throw new UnauthorizedAccessException();
        }

        return accessToken.AccessTokenClaims.Select(a => a.Claim);
    }

    public AuthenticationRequestHandler(IHttpContextAccessor httpContext, IMediator mediator)
    {
        this.httpContext = httpContext;
        this.mediator = mediator;
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
            var claims = await GetClaims(accessToken.FirstOrDefault()!);

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
