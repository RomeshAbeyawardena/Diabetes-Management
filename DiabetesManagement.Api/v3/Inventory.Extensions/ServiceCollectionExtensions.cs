using Inventory.Attributes;
using Inventory.Contracts;
using Inventory.Defaults;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using System.Reflection;
using System.Text.Json;

namespace Inventory.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterCoreServices(this IServiceCollection services, string fileName)
    {
        var moduleData = JsonSerializer.Deserialize<ModuleData>(File.ReadAllText(fileName));
        var modules = new DefaultModuleProvider().GetModules(moduleData.Modules);
        return RegisterCoreServices(services, modules);
    }

    public static IServiceCollection RegisterCoreServices(this IServiceCollection services, IEnumerable<IModule> modules)
    {
        foreach(var module in modules)
        {
            module.RegisterServices(services);
        }

        return RegisterCoreServices(services, modules.SelectMany(m => m.Assemblies).ToArray());
    }

    public static IServiceCollection RegisterCoreServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        return services
            .AddSingleton<ISystemClock, SystemClock>()
            .AddSingleton<ApplicationSettings>()
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
