using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Inventory.Extensions;
using Inventory.Api;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Inventory.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .RegisterCoreServices("modules.json");
        }
    }
}
