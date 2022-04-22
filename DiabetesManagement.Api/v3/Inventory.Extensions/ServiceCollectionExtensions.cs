using DiabetesManagement.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;

namespace DiabetesManagement.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterCoreServices(this IServiceCollection services, params Type[] types)
    {
        return services
            .AddSingleton<ISystemClock, SystemClock>()
            .AddSingleton<ApplicationSettings>()
            .AddMediatR(types)
            .AddAutoMapper(types)
            .Scan(s => s
                .FromAssembliesOf(types)
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
