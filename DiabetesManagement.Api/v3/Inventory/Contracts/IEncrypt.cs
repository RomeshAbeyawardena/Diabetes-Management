namespace Inventory.Contracts;

public interface IEncrypt<T>
{
    void Encrypt(T model);
}
