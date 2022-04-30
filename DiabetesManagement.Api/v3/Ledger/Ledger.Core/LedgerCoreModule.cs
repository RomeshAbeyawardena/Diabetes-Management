using Inventory.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Ledger.Core;
public class LedgerCoreModule : ModuleBase
{
    public LedgerCoreModule(ILogger logger) 
        : base(logger, nameof(LedgerCoreModule), GetAssembly<LedgerCoreModule>())
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
