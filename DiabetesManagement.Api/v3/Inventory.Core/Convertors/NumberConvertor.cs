using Inventory.Attributes;
using Inventory.Contracts;
using System.Text.Json;

namespace Inventory.Core.Convertors;

[RegisterService(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
public class NumberConvertor : IConvertor
{
    private JsonElement element;
    public int OrderIndex => 0;
    public bool CanConvert(JsonElement element)
    {
        this.element = element;
        return element.ValueKind == JsonValueKind.Number;
    }

    public object? Convert()
    {
        var numberValue = this.element.GetRawText();
        if (numberValue.Contains('.'))
        {
            return element.GetDecimal();
        }

        return element.GetUInt64();
    }
}
