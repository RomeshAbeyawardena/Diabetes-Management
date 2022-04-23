using Inventory.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Persistence;

public class PersistenceModule : ModuleBase
{
    public PersistenceModule()
        : base(nameof(PersistenceModule), GetAssembly<PersistenceModule>())
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
