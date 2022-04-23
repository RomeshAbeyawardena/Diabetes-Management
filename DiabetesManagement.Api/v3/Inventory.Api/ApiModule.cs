using Inventory.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Inventory.Api;

public class ApiModule : ModuleBase
{
    public ApiModule(ILogger logger)
        : base(nameof(ApiModule), GetAssembly<ApiModule>())
    {
        logger.Information("Configuring API module...", this);
    }
    public override bool CanRun()
    {
        return true;
    }

    public override void RegisterServices(IServiceCollection services)
    {
        
    }
}
