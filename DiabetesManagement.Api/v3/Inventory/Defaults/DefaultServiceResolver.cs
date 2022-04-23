using Inventory.Contracts;

namespace Inventory.Defaults;

public class DefaultServiceResolver : IServiceResolver
{
    private readonly IEnumerable<IResolveableService> resolveableServices;

    public DefaultServiceResolver(IEnumerable<IResolveableService> resolveableServices)
    {
        this.resolveableServices = resolveableServices;
    }

    public object? ResolveService(Type serviceType)
    {
        foreach(var service in resolveableServices)
        {
            if (service.CanResolve(serviceType))
            {
                return service.Create(serviceType);
            }
        }

        return null;
    }
}
