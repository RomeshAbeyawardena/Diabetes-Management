using Inventory.Attributes;
using Inventory.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Core.Defaults;
[RegisterService(ServiceLifetime.Scoped)]
public class DefaultDbContextProvider : IDbContextProvider
{
    private readonly IServiceProvider serviceProvider;

    public DefaultDbContextProvider(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IDbContext? GetDbContext(Type dbContextType)
    {
        return serviceProvider.GetService(dbContextType) as IDbContext;
    }

    public T? GetDbContext<T>() where T : IDbContext
    {
        return serviceProvider.GetService<T>();
    }
}
