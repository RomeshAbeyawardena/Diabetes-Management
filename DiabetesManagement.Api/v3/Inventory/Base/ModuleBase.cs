using Inventory.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Serilog;

namespace Inventory.Base;

public abstract class ModuleBase : IModule
{
    protected static Assembly GetAssembly<T>()
    {
        return typeof(T).Assembly;
    }

    public ModuleBase(ILogger logger, string name, params Assembly[] assemblies)
    {
        Name = name;
        logger.Information($"Configuring {Name} module...", this);
        Assemblies = assemblies;

    }

    public string Name { get; }

    public IEnumerable<Assembly> Assemblies { get; }

    public abstract bool CanRun();
    public abstract void RegisterServices(IServiceCollection services);
}
