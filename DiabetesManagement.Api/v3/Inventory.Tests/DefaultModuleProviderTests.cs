using Inventory.Base;
using Inventory.Core.Defaults;
using NUnit.Framework;
using System.Linq;
using System.Reflection;

namespace Inventory.Tests;

public class MyModule : ModuleBase
{
    public MyModule() : base(nameof(MyModule))
    {
    }

    public override bool CanRun()
    {
        return true;
    }
}

public class DefaultModuleProviderTests
{
    

    private DefaultModuleProvider? sut;
    public DefaultModuleProviderTests()
    {
        sut = new DefaultModuleProvider();
    }

    [Test]
    public void Test()
    {
        var modules = sut.GetModules(new[] { "Inventory", "Inventory.Core", "Inventory.Persistence" });
        Assert.IsNotEmpty(modules);
        Assert.AreEqual(3, modules.Count());
    }
}
