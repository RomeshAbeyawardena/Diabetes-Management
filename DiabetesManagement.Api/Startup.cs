using DiabetesManagement.Shared;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(DiabetesManagement.Api.Startup))]
namespace DiabetesManagement.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ApplicationSettings>();
        }
    }
}
