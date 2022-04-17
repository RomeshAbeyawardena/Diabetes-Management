using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiabetesManagement.Core.Defaults;
[RegisterService(ServiceLifetime.Scoped)]
public class DefaultDbContextProvider : IDbContextProvider
{
    private readonly IServiceProvider serviceProvider;

    public DefaultDbContextProvider(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public DbContext? GetDbContext(Type dbContextType)
    {
        return serviceProvider.GetService(dbContextType) as DbContext;
    }

    public T? GetDbContext<T>() where T : DbContext
    {
        return serviceProvider.GetService<T>();
    }
}
