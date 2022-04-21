namespace DiabetesManagement.Contracts
{
    public interface IJwtProvider
    {
        string BuildToken(IDictionary<string, object> tokenValues, Microsoft.IdentityModel.Tokens.TokenValidationParameters validationParameters);
        IDictionary<string, string> Extract(string token, Microsoft.IdentityModel.Tokens.TokenValidationParameters validationParameters);
    }
}
