using Microsoft.IdentityModel.Tokens;

namespace Inventory.Contracts
{
    public interface IJwtProvider
    {
        TokenValidationParameters DefaultTokenValidationParameters { get; }
        string BuildToken(IDictionary<string, object> tokenValues, TokenValidationParameters validationParameters);
        IDictionary<string, string> Extract(string token, TokenValidationParameters validationParameters);
    }
}
