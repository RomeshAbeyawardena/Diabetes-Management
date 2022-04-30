using Inventory.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Ledger;
public class LedgerModule : ModuleBase
{
    public LedgerModule()
        : base(nameof(LedgerModule), GetAssembly<LedgerModule>())
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
