using Inventory.Contracts;
using System.Text.Json;

namespace Inventory.Extensions;

public static class ConvertorFactoryExtensions
{
    public static object? Convert(this IConvertorFactory convertorFactory, JsonElement element)
    {
        var convertor = convertorFactory.GetConvertor(element);
        if (convertor != null)
        {
            return convertor.Convert();
        }

        return null;
    }
}
