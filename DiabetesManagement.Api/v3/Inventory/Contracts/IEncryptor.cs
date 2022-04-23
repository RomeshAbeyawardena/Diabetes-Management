namespace Inventory.Contracts;

public interface IEncryptor<T>
{
    void Encrypt(T model);
}
