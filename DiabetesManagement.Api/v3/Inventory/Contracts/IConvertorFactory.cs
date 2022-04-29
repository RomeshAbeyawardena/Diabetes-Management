using System.Text.Json;

namespace Inventory.Contracts;

public interface IConvertorFactory
{
    IConvertor? GetConvertor(JsonElement element);
}
