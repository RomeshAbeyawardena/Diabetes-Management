using Inventory.Attributes;
using Inventory.Contracts;
using System.Text.Json;

namespace Inventory.Core.Convertors;

[RegisterService]
public class TimeSpanConvertor : IConvertor
{
    private TimeSpan timeSpan;
    public int OrderIndex => 0;
    public bool CanConvert(JsonElement element)
    {
        return element.ValueKind == JsonValueKind.String 
            && TimeSpan.TryParse(element.GetRawText(), out timeSpan);
    }

    public object? Convert()
    {
        return timeSpan;
    }
}
