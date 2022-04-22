using Inventory.Contracts;
using System.Reflection;

namespace Inventory.Base;

public abstract class ModuleBase : IModule
{
    protected static Assembly GetAssembly<T>()
    {
        return typeof(T).Assembly;
    }

    public ModuleBase(string name, params Assembly[] assemblies)
    {
        Name = name;
        Assemblies = assemblies;
    }

    public string Name { get; }

    public IEnumerable<Assembly> Assemblies { get; }

    public abstract bool CanRun();
}
