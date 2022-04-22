using Inventory.Attributes;
using Inventory.Contracts;

namespace Inventory.Core.Convertors;

[RegisterService]
public class GuidConvertor : IConvertor
{
    private Guid? value;
    public bool CanConvert(Type type, object value)
    {
        if (type != typeof(Guid) && type != typeof(Guid?))
        {
            return false;
        }

        if (Guid.TryParse(value.ToString(), out var result))
        {
            this.value = result;
            return true;
        }

        return false;
    }

    public object? Convert()
    {
        return value;
    }
}
