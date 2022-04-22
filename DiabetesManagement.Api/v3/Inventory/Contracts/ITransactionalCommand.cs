namespace Inventory.Contracts;

public interface ITransactionalCommand<T>
    where T : class
{
    T? Model { get; }
    bool CommitChanges { get; set; }
}
