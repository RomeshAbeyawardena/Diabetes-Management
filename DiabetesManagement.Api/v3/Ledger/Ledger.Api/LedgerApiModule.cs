using Inventory.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace Ledger.Api;
public class LedgerApiModule : ModuleBase
{
    public LedgerApiModule(ILogger logger) : base(logger, nameof(LedgerApiModule), GetAssembly<LedgerApiModule>())
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
