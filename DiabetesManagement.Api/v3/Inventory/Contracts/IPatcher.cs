namespace Inventory.Contracts;

public interface IPatcher
{
    public T Apply<T>(IEnumerable<IPatchOperation> operations);
}
