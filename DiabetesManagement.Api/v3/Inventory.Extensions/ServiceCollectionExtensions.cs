using Inventory.Attributes;
using Inventory.Contracts;
using Inventory.Defaults;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;

namespace Inventory.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterCoreServices(this IServiceCollection services, string fileName)
    {
        var moduleData = JsonSerializer.Deserialize<ModuleData>(File.ReadAllText(fileName));

        if (moduleData == null || moduleData.Modules == null || !moduleData.Modules.Any())
        {
            throw new NullReferenceException();
        }

        using var moduleProvider = new DefaultModuleProvider();
        var modules = moduleProvider.GetModules(moduleData.Modules);

        if(modules == null)
        {
            throw new NullReferenceException();
        }

        return RegisterCoreServices(services, modules);
    }

    public static IServiceCollection RegisterCoreServices(this IServiceCollection services, IEnumerable<IModule> modules)
    {
        var assemblies = new List<Assembly>();
        foreach(var module in modules)
        {
            module.RegisterServices(services);
            assemblies.AddRange(module.Assemblies);
        }

        return RegisterCoreServices(services, assemblies.ToArray());
    }

    public static IServiceCollection RegisterCoreServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        return services
            .AddMediatR(assemblies)
            .AddAutoMapper(assemblies)
            .Scan(s => s
                .FromAssemblies(assemblies)
                .AddClasses(c => c.WithAttribute<RegisterServiceAttribute>(s => s.ServiceLifetime == ServiceLifetime.Singleton))
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
                .AddClasses(c => c.WithAttribute<RegisterServiceAttribute>(s => s.ServiceLifetime == ServiceLifetime.Scoped))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(c => c.WithAttribute<RegisterServiceAttribute>(s => s.ServiceLifetime == ServiceLifetime.Transient))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
    }
}
