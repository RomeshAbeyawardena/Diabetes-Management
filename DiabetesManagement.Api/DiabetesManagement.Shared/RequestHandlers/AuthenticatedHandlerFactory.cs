using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Contracts;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;

namespace DiabetesManagement.Shared.RequestHandlers
{
    public class AuthenticatedHandlerFactory : HandlerFactory, IAuthenticatedHandlerFactory
    {
        private Models.ApiToken? apiToken;
        private IEnumerable<Models.ApiTokenClaim> claims = Array.Empty<Models.ApiTokenClaim>();
        private bool BypassChecks = false;
        public AuthenticatedHandlerFactory(string connectionString, ILogger logger, IEnumerable<Assembly>? assemblies = null) 
            : base(connectionString, logger, assemblies)
        {
        }

        public AuthenticatedHandlerFactory(ILogger logger, IDbConnection dbConnection, IDbTransaction? dbTransaction = null, IEnumerable<Assembly>? assemblies = null) 
            : base(logger, dbConnection, dbTransaction, assemblies)
        {
        }

        internal override async Task<IRequestHandler> GetRequestHandler(string queryOrCommand)
        {
            var requestHandler = await base.GetRequestHandler(queryOrCommand);
            if (BypassChecks)
            {
                return requestHandler;
            }

            var handlerDescriptor = requestHandler.GetType().GetCustomAttribute<HandlerDescriptorAttribute>();

            if (handlerDescriptor != null 
                    && apiToken != null 
                    && claims != null && claims.Any())
            {
                if(!handlerDescriptor.RequiredPermissions.Any() 
                    || claims.Any(c => handlerDescriptor.RequiredPermissions.Contains(c.Claim)))
                {
                    return requestHandler;
                }

                throw new UnauthorizedAccessException();
            }

            throw new UnauthorizedAccessException("Authentication has not been carried out, call IsAuthenticated(string key, string secret) and try again");
        }

        public async Task<bool> IsAuthenticated(string key, string secret)
        {
            return await BypassAuthentication(async (handler) =>
            {
                apiToken = await Execute<ApiToken.GetRequest, Models.ApiToken>(
                    ApiToken.Queries.GetApiToken,
                    new ApiToken.GetRequest
                    {
                        Key = key,
                        Secret = secret
                    });

                if (apiToken == null)
                {
                    return false;
                }

                claims = await Execute<ApiTokenClaim.GetRequest, IEnumerable<Models.ApiTokenClaim>>(
                    ApiTokenClaim.Queries.GetApiTokenClaimsQuery,
                    new ApiTokenClaim.GetRequest
                    {
                        ApiTokenId = apiToken.ApiTokenId
                    });

                return await IsAuthenticated(apiToken);
            });
        }

        public async Task<bool> IsAuthenticated(Models.ApiToken apiToken)
        {
            return await BypassAuthentication(async (handler) =>
            {
                if (apiToken == null)
                {
                    return false;
                }

                claims = await handler.Execute<ApiTokenClaim.GetRequest, IEnumerable<Models.ApiTokenClaim>>(
                    ApiTokenClaim.Queries.GetApiTokenClaimsQuery,
                    new ApiTokenClaim.GetRequest
                    {
                        ApiTokenId = apiToken.ApiTokenId
                    });

                BypassChecks = false;
                return true;
            });
        }

        public async Task BypassAuthentication(Func<IAuthenticatedHandlerFactory, Task> action)
        {
            if (!BypassChecks)
            {
                BypassChecks = true;
            }

            await action(this);

            BypassChecks = false;
        }

        public async Task<TResult> BypassAuthentication<TResult>(Func<IAuthenticatedHandlerFactory, Task<TResult>> action)
        {
            if (!BypassChecks)
            {
                BypassChecks = true;
            }

            var result = await action(this);

            BypassChecks = false;

            return result;
        }
    }
}
