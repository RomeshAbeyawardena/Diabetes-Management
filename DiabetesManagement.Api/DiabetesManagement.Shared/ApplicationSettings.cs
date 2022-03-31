using Microsoft.Extensions.Configuration;

namespace DiabetesManagement.Shared
{
    public class ApplicationSettings
    {
        public ApplicationSettings(IConfiguration configuration)
        {
            configuration.Bind(this);
            Instance = this;
        }

        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? SigningKey { get; set; }
        public string? ServerKey { get; set; }
        public string? ServerInitialVector { get; set; }
        public static ApplicationSettings? Instance { get; set; }
    }
}
