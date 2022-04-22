using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Inventory.Persistence.Base;
using Inventory.Extensions;
using Inventory.Api;
using Inventory.Persistence;
using Inventory.Core.Features.InventoryHistory;
using Inventory.Core.Features;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Inventory.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .RegisterCoreServices(typeof(Startup), typeof(Get), typeof(RepositoryBase<,>))
                .RegisterDbServices()
                .AddTransient(typeof(MediatR.Pipeline.IRequestPreProcessor<>), typeof(AuthenticationRequestHandler<>));
        }
    }
}
