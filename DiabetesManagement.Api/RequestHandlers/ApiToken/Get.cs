using System.Data;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers.ApiToken
{
    using DiabetesManagement.Shared.Attributes;
    using DiabetesManagement.Shared.Contracts;
    using DiabetesManagement.Shared.RequestHandlers;
    using ApiTokenFeature = Shared.RequestHandlers.ApiToken;
    using Models = Shared.Models;

    [HandlerDescriptor(Queries.GetValidatedApiToken)]
    public class Get : HandlerBase<GetRequest, Models.ApiToken>
    {
        public Get(string connectionString) : base(connectionString)
        {
        }

        public Get(IDbConnection dbConnection, IDbTransaction dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        protected override Task<Models.ApiToken> HandleAsync(GetRequest request)
        {
            var secret = ""; //extract secret;

            Task<Models.ApiToken> GetApiToken(IHandlerFactory handlerFactory)
            {
                return HandlerFactory
                     .Execute<ApiTokenFeature.GetRequest, Models.ApiToken>(
                         ApiTokenFeature.Queries.GetApiToken,
                         new ApiTokenFeature.GetRequest
                         {
                             Key = request.ApiKey,
                             Secret = secret
                         });
            }

            if(request.UseAuthenticatedContext && HandlerFactory is IAuthenticatedHandlerFactory authenticatedHandlerFactory)
            {
                return authenticatedHandlerFactory
                    .BypassAuthentication(async (handlerFactory) => await GetApiToken(authenticatedHandlerFactory));
            }

            return GetApiToken(HandlerFactory);
        }

        protected override void Dispose(bool disposing)
        {

        }

    }
}
