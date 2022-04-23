using Inventory.Contracts;
using Serilog;
using Serilog.Core;
using System.Reflection;

namespace Inventory.Defaults;

public class DefaultModuleProvider : IModuleProvider
{
    private readonly Logger logger;
    private IEnumerable<Assembly>? assemblies;
    private IServiceResolver? serviceResolver;
    private static bool HasInterface(Type type, Type interfaceType)
    {
        return !type.IsAbstract && type.GetInterfaces().Any(i => i == interfaceType);
    }

    private bool CanResolve(Type type, out object? implementation)
    {
        serviceResolver ??= new DefaultServiceResolver(
            assemblies
                .SelectMany(a => a.GetTypes().Where(t => HasInterface(t, typeof(IResolveableService))))
                .Select(t => (IResolveableService)Activator.CreateInstance(t))
            );

        implementation = null;
        if (type == typeof(ILogger))
        {
            implementation = logger;
            return true;
        }
        
        implementation = serviceResolver.ResolveService(type);

        return implementation != null;
    }

    private bool ResolveParameters(IEnumerable<ParameterInfo> parameters, out IEnumerable<object> constructorArguments)
    {
        var constructorArgumentsList = new List<object>();
        constructorArguments = constructorArgumentsList;
        if (parameters.Any())
        {
            foreach(var parameter in parameters)
            {
                if(!CanResolve(parameter.ParameterType, out var implementation))
                {
                    return false;
                }

                constructorArgumentsList.Add(implementation!);
            }

            return true;
        }

        return false;
    }

    void IDisposable.Dispose()
    {
        logger.Information("Disposing of services used by this instance of module provider");
        logger.Dispose();
        GC.SuppressFinalize(this);
    }

    public DefaultModuleProvider()
    {
        logger = new LoggerConfiguration()
            .WriteTo
            .Console()
            .CreateLogger();
    }

    public IModule? GetModuleFromType(Type type)
    {
        IEnumerable<object> constructorArguments = Array.Empty<object>();
        var ctor = type.GetConstructors()
            .OrderByDescending(p => p.GetParameters().Length)
            .FirstOrDefault(a => ResolveParameters(a.GetParameters(), out constructorArguments));

        var instance = Activator.CreateInstance(type, constructorArguments.ToArray());

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
        assemblies = assemblyNames.Select(a => Assembly.Load(a));
        var moduleTypes = assemblies.SelectMany(a => a.GetTypes().Where(t => HasInterface(t, typeof(IModule))));

        var modules = moduleTypes.Select(t => GetModuleFromType(t));

        return modules.Where(m => m!.CanRun())!;
    }
}
