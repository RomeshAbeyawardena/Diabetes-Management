using Inventory.Attributes;
using Inventory.Contracts;
using System.Globalization;
using System.Text.Json;

namespace Inventory.Core.Convertors;

[RegisterService(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
public class DateTimeOffsetConvertor : IConvertor
{
    private DateTimeOffset date;
    public int OrderIndex => 0;
    public bool CanConvert(JsonElement element)
    {
        return (element.ValueKind == JsonValueKind.String && DateTimeOffset.TryParseExact(element.GetString(), "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out date));
    }

    public object? Convert()
    {
        return date;
    }
}
