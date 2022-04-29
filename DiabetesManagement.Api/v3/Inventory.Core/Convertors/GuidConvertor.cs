using Inventory.Attributes;
using Inventory.Contracts;
using System.Text.Json;

namespace Inventory.Core.Convertors;

[RegisterService(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
public class GuidConvertor : IConvertor
{
    private Guid value;
    public int OrderIndex => 0;
    public bool CanConvert(JsonElement element)
    {
        if (!(element.ValueKind == JsonValueKind.String 
            && Guid.TryParse(element.GetString(), out value)))
        {
            return false;
        }

        return true;
    }

    public object? Convert()
    {
        return value;
    }
}
