using Inventory.Attributes;
using Inventory.Contracts;
using System.Text.Json;

namespace Inventory.Core.Convertors;

[RegisterService(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
public class BooleanConvertor : IConvertor
{
    private JsonElement? element;
    public int OrderIndex => 0;
    public bool CanConvert(JsonElement element)
    {
        this.element = element;
        return element.ValueKind == JsonValueKind.True ||
            element.ValueKind == JsonValueKind.False;
    }

    public object? Convert()
    {
        if (element.HasValue)
        {
            return element.Value.GetBoolean();
        }

        return false;
    }
}
