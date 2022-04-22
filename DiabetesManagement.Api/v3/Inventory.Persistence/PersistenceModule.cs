using Inventory.Base;

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
}
