using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using DiabetesManagement.Extensions;
using DiabetesManagement.Core.Features.Inventory;

[assembly: FunctionsStartup(typeof(DiabetesManagement.Api.Startup))]
namespace DiabetesManagement.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.RegisterCoreServices(typeof(Startup), typeof(Get));
        }
    }
}
