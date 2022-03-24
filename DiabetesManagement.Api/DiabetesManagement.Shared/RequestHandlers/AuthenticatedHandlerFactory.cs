using DiabetesManagement.Shared.Contracts;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;

namespace DiabetesManagement.Shared.RequestHandlers
{
    public class AuthenticatedHandlerFactory : HandlerFactory
    {
        private Models.ApiToken? apiToken;
        private IEnumerable<Models.ApiTokenClaim> claims = Array.Empty<Models.ApiTokenClaim>();

        public AuthenticatedHandlerFactory(string connectionString, ILogger logger, IEnumerable<Assembly>? assemblies = null) : base(connectionString, logger, assemblies)
        {
        }

        public AuthenticatedHandlerFactory(ILogger logger, IDbConnection dbConnection, IDbTransaction? dbTransaction = null, IEnumerable<Assembly>? assemblies = null) : base(logger, dbConnection, dbTransaction, assemblies)
        {
        }

        internal override IRequestHandler<TRequest> GetRequestHandler<TRequest>(string queryOrCommand)
        {
            return base.GetRequestHandler<TRequest>(queryOrCommand);
        }

        public async Task<bool> IsAuthenticated(string key, string secret)
        {
            apiToken = await Execute<ApiToken.GetRequest, Models.ApiToken> (
                ApiToken.Queries.GetApiToken, 
                new ApiToken.GetRequest { Key = key, Secret = secret });

            if(apiToken == null)
            {
                return false;
            }

            claims = await Execute<ApiTokenClaim.GetRequest, IEnumerable<Models.ApiTokenClaim>> (
                ApiTokenClaim.Queries.GetApiTokenClaimsQuery, 
                new ApiTokenClaim.GetRequest
                {
                    ApiTokenId = apiToken.ApiTokenId
                });

            return true;
        }
    }
}
