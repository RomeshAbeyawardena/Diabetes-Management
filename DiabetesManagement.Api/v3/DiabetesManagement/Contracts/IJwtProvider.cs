namespace DiabetesManagement.Contracts
{
    public interface IJwtProvider
    {
        IDictionary<string, string> Extract(string token, Microsoft.IdentityModel.Tokens.TokenValidationParameters validationParameters);
    }
}
