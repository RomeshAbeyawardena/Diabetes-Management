using Inventory.Base;

namespace Inventory.Core;
public class CoreModule : ModuleBase
{
    public CoreModule()
        : base(nameof(CoreModule), GetAssembly<CoreModule>())
    {

    }

    public override bool CanRun()
    {
        return true;
    }
}
