using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers.ApiToken
{
    using DiabetesManagement.Shared.Attributes;
    using DiabetesManagement.Shared.Contracts;
    using DiabetesManagement.Shared.RequestHandlers;
    using System.Linq;
    using ApiTokenFeature = Shared.RequestHandlers.ApiToken;
    using Models = Shared.Models;

    [HandlerDescriptor(Queries.GetValidatedApiToken)]
    public class Get : HandlerBase<GetRequest, Models.ApiToken>
    {
        private async Task<TokenValidationResult> ReadJwt(string token, TokenValidationParameters tokenValidationParameters)
        {
            var tokenHandler = new JsonWebTokenHandler();
            if (!tokenHandler.CanReadToken(token))
            {
                throw new ArgumentException("", nameof(token));
            }

            var result = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);

            return result;
        }

        public Get(string connectionString) : base(connectionString)
        {
        }

        public Get(IDbConnection dbConnection, IDbTransaction dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        protected override async Task<Models.ApiToken> HandleAsync(GetRequest request)
        {
            SymmetricSecurityKey symmetricSecurityKey = new (request.Key.ToArray());

            var result = await ReadJwt(request.ApiKeyChallenge, new TokenValidationParameters
            {
                ValidAudience = request.ValidAudience,
                ValidIssuer = request.ValidIssuer,
                IssuerSigningKey = symmetricSecurityKey
            });

            var claims = result.Claims;

            if (!claims.TryGetValue("api-key", out var key) || !key.Equals(request.Key))
            {
                throw new UnauthorizedAccessException();
            }

            if(!claims.TryGetValue("secret", out var secret))
            {
                throw new UnauthorizedAccessException();
            } //extract secret;

            Task<Models.ApiToken> GetApiToken(IHandlerFactory handlerFactory)
            {
                return HandlerFactory
                     .Execute<ApiTokenFeature.GetRequest, Models.ApiToken>(
                         ApiTokenFeature.Queries.GetApiToken,
                         new ApiTokenFeature.GetRequest
                         {
                             Key = request.ApiKey,
                             Secret = secret.ToString()
                         });
            }

            if(request.UseAuthenticatedContext && HandlerFactory is IAuthenticatedHandlerFactory authenticatedHandlerFactory)
            {
                return await authenticatedHandlerFactory
                    .BypassAuthentication(async (handlerFactory) => await GetApiToken(authenticatedHandlerFactory));
            }

            return await GetApiToken(HandlerFactory);
        }

        protected override void Dispose(bool disposing)
        {

        }

    }
}
