namespace Inventory.Contracts
{
    public interface IServiceResolver
    {
        object? ResolveService(Type serviceType);
    }
}
