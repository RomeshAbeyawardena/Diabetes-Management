using Inventory.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ledger;
public class LedgerModule : ModuleBase
{
    public LedgerModule(Serilog.ILogger logger)
        : base(logger, nameof(LedgerModule), GetAssembly<LedgerModule>())
    {

    }

    public override bool CanRun()
    {
        return true;
    }

    public override void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        
    }
}
