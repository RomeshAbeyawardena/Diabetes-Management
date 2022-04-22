using Inventory.Attributes;
using Inventory.Contracts;

namespace Inventory.Core.Convertors;

[RegisterService]
public class StringConvertor : IConvertor
{
    private string? value;
    public bool CanConvert(Type type, object value)
    {
        if (type != typeof(string))
        {
            return false;
        }

        this.value = value.ToString()!;
        return true;
    }

    public object? Convert()
    {
        return value;
    }
}
