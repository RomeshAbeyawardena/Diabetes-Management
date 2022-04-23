namespace Inventory.Contracts;

public interface IResolveableService
{
    Type ServiceType { get; }
    Type ImplementationType { get; }
    object Create(Type serviceType, IEnumerable<object> arguments);
}
