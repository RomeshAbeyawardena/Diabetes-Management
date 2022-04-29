using System.Text.Json;

namespace Inventory.Contracts;

public interface IConvertor
{
    int OrderIndex { get; }
    bool CanConvert(JsonElement element);
    object? Convert();
}
