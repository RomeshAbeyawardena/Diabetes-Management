namespace Inventory.Contracts;

public interface IResolveableService
{
    Type ServiceType { get; }
    Type ImplementationType { get; }
    bool CanResolve(Type serviceType);
    object Create(Type serviceType);
}
