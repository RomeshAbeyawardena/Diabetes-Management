using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DiabetesManagement.Core.Defaults
{
    [RegisterService]
    public class DefaultJwtProvider : IJwtProvider
    {
        private readonly JwtSecurityTokenHandler handler;
        private readonly IClockProvider clockProvider;
        private readonly ApplicationSettings applicationSettings;

        public DefaultJwtProvider(IClockProvider clockProvider, ApplicationSettings applicationSettings)
        {
            handler = new JwtSecurityTokenHandler();
            this.clockProvider = clockProvider;
            this.applicationSettings = applicationSettings;
        }

        public string BuildToken(IDictionary<string, object> tokenValues, TokenValidationParameters validationParameters)
        {
            IdentityModelEventSource.ShowPII = true;

            var key = new SymmetricSecurityKey(applicationSettings.ConfidentialServerKeyBytes);
            var key2 = new SymmetricSecurityKey(applicationSettings.PersonalDataServerKeyBytes);
            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Audience = validationParameters.ValidAudience,
                Claims = tokenValues,
                Issuer = validationParameters.ValidIssuer,
                Expires = clockProvider.Clock.UtcNow.AddHours(4).DateTime,
                NotBefore = clockProvider.Clock.UtcNow.DateTime,
                
                EncryptingCredentials = new EncryptingCredentials(key, SecurityAlgorithms.Aes256KeyWrap, SecurityAlgorithms.Aes256CbcHmacSha512),
                SigningCredentials = new SigningCredentials(key2, SecurityAlgorithms.HmacSha512Signature),
                IssuedAt = clockProvider.Clock.UtcNow.DateTime,
            });
            
            
            return handler.WriteToken(token);
        }

        public IDictionary<string, string> Extract(string token, Microsoft.IdentityModel.Tokens.TokenValidationParameters validationParameters)
        {
            
            var s = handler.ValidateToken(token, validationParameters, out var securityToken);
            return s.Claims.ToDictionary(c => c.Type, c => c.Value);
            
        }
    }
}
