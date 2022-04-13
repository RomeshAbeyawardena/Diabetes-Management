using Microsoft.Extensions.Configuration;

namespace DiabetesManagement;
public class ApplicationSettings
{
    public ApplicationSettings(IConfiguration configuration)
    {
        configuration.Bind(this);
        DefaultConnectionString = configuration.GetConnectionString("default");
    }
    public string DefaultConnectionString { get; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? SigningKey { get; set; }
    public string? PersonalDataServerKey { get; set; }
    public string? ConfidentialServerKey { get; set; }
    public string? ServerInitialVector { get; set; }
}
