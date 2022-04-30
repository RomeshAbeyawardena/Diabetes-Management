using Inventory.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

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

    public override void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.RegisterDbServices();
    }
}
