using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.RequestHandlers;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers.ApiToken
{
    [HandlerDescriptor(Commands.SaveApiTokenChallenge)]
    public class Post : HandlerBase<SaveRequest, string>
    {
        private static string GenerateToken(SaveRequest request)
        {
            SymmetricSecurityKey symmetricSecurityKey = new(request.Key.ToArray());
            IdentityModelEventSource.ShowPII = true;

            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var encryptingCredentials = new EncryptingCredentials(symmetricSecurityKey, SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512);

            var tokenHandler = new JsonWebTokenHandler();

            Dictionary<string, object> claims = new();

            claims.Add("apiKey", request.ApiKey);
            claims.Add("secret", request.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor {
                EncryptingCredentials = encryptingCredentials,
                SigningCredentials = signingCredentials,
                Claims = claims,
                Audience = request.Audience,
                Issuer = request.Issuer 
            });

            return token;
        }

        public Post(string connectionString) : base(connectionString)
        {
        }

        public Post(IDbConnection dbConnection, IDbTransaction dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        protected override void Dispose(bool disposing)
        {
            
        }

        protected override async Task<string> HandleAsync(SaveRequest request)
        {
            await Task.CompletedTask;
            return GenerateToken(request);
        }

        

    }
}
