using Inventory.Base;

namespace Inventory.WebApi;

public class WebApiModule : ModuleBase
{
    public WebApiModule()
        : base(nameof(WebApiModule), GetAssembly<WebApiModule>())
    {

    }

    public override bool CanRun()
    {
        return true;
    }

    public override void RegisterServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor()
            .AddControllers();
    }
}
