using Microsoft.Extensions.Configuration;

namespace DiabetesManagement.Shared
{
    public class ApplicationSettings
    {
        public ApplicationSettings(IConfiguration configuration)
        {
            configuration.Bind(this);
        }

        public string? SigningKey { get; set; }
    }
}
