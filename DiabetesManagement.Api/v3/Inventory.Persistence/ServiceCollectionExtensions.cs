using DiabetesManagement.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiabetesManagement.Persistence;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDbServices(this IServiceCollection services)
    {
        return services
            .AddDbContext<InventoryDbContext>(ConfigureDbContext)
            .AddTransient<IInventoryDbContext>(s => s.GetRequiredService<InventoryDbContext>());
    }


    private static void ConfigureDbContext(IServiceProvider serviceProvider, DbContextOptionsBuilder builder)
    {
        var applicationSettings = serviceProvider.GetService<ApplicationSettings>();
        builder.UseSqlServer(applicationSettings!.DefaultConnectionString);
    }
}
