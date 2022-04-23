namespace Inventory.Contracts;

public interface IDecryptor<T>
{
    void Decrypt(T model);
}
