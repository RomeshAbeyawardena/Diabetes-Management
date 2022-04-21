using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DiabetesManagement.Core.Defaults
{
    [RegisterService]
    public class DefaultJwtProvider : IJwtProvider
    {
        public IDictionary<string, string> Extract(string token, Microsoft.IdentityModel.Tokens.TokenValidationParameters validationParameters)
        {
            var handler = new JwtSecurityTokenHandler();
            var s = handler.ValidateToken(token, validationParameters, out var securityToken);
            return s.Claims.ToDictionary(c => c.Type, c => c.Value);
            
        }
    }
}
