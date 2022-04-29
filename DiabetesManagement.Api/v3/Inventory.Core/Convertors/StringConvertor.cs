using Inventory.Attributes;
using Inventory.Contracts;
using System.Text.Json;

namespace Inventory.Core.Convertors;

[RegisterService]
public class StringConvertor : IConvertor
{
    private JsonElement element;
    public int OrderIndex => int.MaxValue;
    public bool CanConvert(JsonElement element)
    {
        this.element = element;
        return element.ValueKind == JsonValueKind.String;
    }

    public object? Convert()
    {
        return element.GetString();
    }
}
