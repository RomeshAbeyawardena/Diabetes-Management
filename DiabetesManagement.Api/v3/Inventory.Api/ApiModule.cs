using Inventory.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Api;

public class ApiModule : ModuleBase
{
    public ApiModule()
        : base(nameof(ApiModule), GetAssembly<ApiModule>())
    {

    }
    public override bool CanRun()
    {
        return true;
    }

    public override void RegisterServices(IServiceCollection services)
    {
        
    }
}
