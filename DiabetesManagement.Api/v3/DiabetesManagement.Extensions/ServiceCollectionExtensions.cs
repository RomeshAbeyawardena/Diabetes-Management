using DiabetesManagement.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiabetesManagement.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterCoreServices(this IServiceCollection services, params Type[] types)
    {
        return services
            .AddSingleton<ApplicationSettings>()
            .AddDbContext<InventoryDbContext>(ConfigureDbContext)
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

    private static void ConfigureDbContext(IServiceProvider serviceProvider, DbContextOptionsBuilder builder)
    {
        var applicationSettings = serviceProvider.GetService<ApplicationSettings>();
        builder.UseSqlServer(applicationSettings!.DefaultConnectionString);
    }
}
