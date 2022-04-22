namespace Inventory.Contracts;

public interface IConvertorFactory
{
    IConvertor? GetConvertor(Type type, object value);
    IConvertor? GetConvertor<T>(T value);
}
