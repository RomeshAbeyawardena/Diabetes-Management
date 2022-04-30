using Inventory.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Ledger.Core;
public class LedgerCoreModule : ModuleBase
{
    public LedgerCoreModule() 
        : base(nameof(LedgerCoreModule), GetAssembly<LedgerCoreModule>())
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
