namespace Inventory.Contracts;

public interface IDecrypt<T>
{
    void Decrypt(T model);
}
