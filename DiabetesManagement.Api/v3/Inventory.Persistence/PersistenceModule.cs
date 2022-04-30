using Inventory.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Inventory.Persistence;

public class PersistenceModule : ModuleBase
{
    public PersistenceModule(ILogger logger)
        : base(logger, nameof(PersistenceModule), GetAssembly<PersistenceModule>())
    {
        
    }

    public override bool CanRun()
    {
        return true;
    }

    public override void RegisterServices(IServiceCollection services)
    {
        services.RegisterDbServices();
    }
}
