using Inventory.Contracts;
using System.Reflection;

namespace Inventory.Defaults;

public class DefaultModuleProvider : IModuleProvider
{
    public IEnumerable<IModule>? GetModules(IEnumerable<string> assemblyNames)
    {
        var assemblies = assemblyNames.Select(a => Assembly.Load(a)).Append(Assembly.GetCallingAssembly());

        var moduleTypes = assemblies.SelectMany(a => a.GetTypes().Where(t => !t.IsAbstract && t.GetInterfaces().Any(i => i == typeof(IModule))));

        var modules = moduleTypes.Select(m => (IModule)Activator.CreateInstance(m));

        return modules.Where(m => m.CanRun())!;
    }
}
