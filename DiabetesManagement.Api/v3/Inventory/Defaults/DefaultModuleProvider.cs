using Inventory.Contracts;
using System.Reflection;

namespace Inventory.Defaults;

public class DefaultModuleProvider : IModuleProvider
{
    public IModule? GetModuleFromType(Type type)
    {
        var instance = Activator.CreateInstance(type);

        if (instance == null)
        {
            throw new InvalidOperationException();
        }

        return (IModule)instance;
    }

    public IEnumerable<IModule?>? GetModulesFromTypes(IEnumerable<string> assemblyNames)
    {
        return assemblyNames.Select(a => { 
            var type = Type.GetType(a);
            if (type == null)
            {
                throw new NullReferenceException();
            }

            return GetModuleFromType(type); }).Where(m => m != null);
    }
    public IEnumerable<IModule>? GetModules(IEnumerable<string> assemblyNames)
    {
        var assemblies = assemblyNames.Select(a => Assembly.Load(a));
        var moduleTypes = assemblies.SelectMany(a => a.GetTypes().Where(t => !t.IsAbstract && t.GetInterfaces().Any(i => i == typeof(IModule))));

        var modules = moduleTypes.Select(t => GetModuleFromType(t));

        return modules.Where(m => m!.CanRun())!;
    }
}
