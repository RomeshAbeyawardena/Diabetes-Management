using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiabetesManagement.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<ApplicationSettings>()
            .AddDbContext<InventoryDbContext>(ConfigureDbContext);
    }

    private static void ConfigureDbContext(IServiceProvider serviceProvider, DbContextOptionsBuilder builder)
    {
        var applicationSettings = serviceProvider.GetService<ApplicationSettings>();
        builder.UseSqlServer(applicationSettings!.DefaultConnectionString);
    }
}
