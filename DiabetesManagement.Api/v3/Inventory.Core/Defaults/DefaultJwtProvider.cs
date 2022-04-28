using Inventory.Attributes;
using Inventory.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Inventory.Core.Defaults
{
    [RegisterService]
    public class DefaultJwtProvider : IJwtProvider
    {
        private readonly JwtSecurityTokenHandler handler;
        private readonly IClockProvider clockProvider;
        private readonly ApplicationSettings applicationSettings;
        private readonly SymmetricSecurityKey key1;
        private readonly SymmetricSecurityKey key2;

        public DefaultJwtProvider(IClockProvider clockProvider, ApplicationSettings applicationSettings)
        {
            handler = new JwtSecurityTokenHandler();
            this.clockProvider = clockProvider;
            this.applicationSettings = applicationSettings;

            key1 = new SymmetricSecurityKey(applicationSettings.ConfidentialServerKeyBytes);
            key2 = new SymmetricSecurityKey(applicationSettings.PersonalDataServerKeyBytes);

            DefaultTokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = applicationSettings.Audience,
                ValidateIssuer = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidIssuer = applicationSettings.Issuer,
                TokenDecryptionKey = key1,
                IssuerSigningKey = key2,
                ValidateIssuerSigningKey = true
            };
        }

        public TokenValidationParameters DefaultTokenValidationParameters { get; }

        public string BuildToken(IDictionary<string, object> tokenValues, TokenValidationParameters validationParameters, bool enforceStandardSettings)
        {
            if (enforceStandardSettings)
            {
                validationParameters.ValidIssuer = applicationSettings.Issuer;
                validationParameters.ValidAudience = applicationSettings.Audience;
                validationParameters.TokenDecryptionKey = key1;
                validationParameters.IssuerSigningKey = key2;
                validationParameters.ValidateIssuerSigningKey = true;
            }

            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Audience = validationParameters.ValidAudience,
                Claims = tokenValues,
                Issuer = validationParameters.ValidIssuer,
                Expires = validationParameters.RequireExpirationTime 
                    ? clockProvider.Clock.UtcNow.Add(applicationSettings.DefaultApplicationExpiry ?? TimeSpan.FromHours(4)).DateTime 
                    : null,
                NotBefore = validationParameters.RequireExpirationTime 
                    ? clockProvider.Clock.UtcNow.DateTime 
                    : null,
                EncryptingCredentials = new EncryptingCredentials(key1, SecurityAlgorithms.Aes256KeyWrap, SecurityAlgorithms.Aes256CbcHmacSha512),
                SigningCredentials = new SigningCredentials(key2, SecurityAlgorithms.HmacSha512Signature),
                IssuedAt = clockProvider.Clock.UtcNow.DateTime,
            });

            return handler.WriteToken(token);
        }

        public IDictionary<string, string> Extract(string token, TokenValidationParameters validationParameters, bool enforceStandardSettings)
        {
            if (enforceStandardSettings)
            {
                validationParameters.ValidIssuer = applicationSettings.Issuer;
                validationParameters.ValidAudience = applicationSettings.Audience;
                validationParameters.TokenDecryptionKey = key1;
                validationParameters.IssuerSigningKey = key2;
                validationParameters.ValidateIssuerSigningKey = true;
            }

            var s = handler.ValidateToken(token, validationParameters, out var securityToken);
            return s.Claims.ToDictionary(c => c.Type, c => c.Value);

        }
    }
}
