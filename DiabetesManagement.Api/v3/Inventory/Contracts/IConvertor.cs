using System.Text.Json;

namespace Inventory.Contracts;
/// <summary>
/// Represents a convertor that can convert <see cref="JsonElement"/> into a CLR type
/// </summary>
public interface IConvertor
{
    /// <summary>
    /// Gets the order this convertor will be executed in
    /// </summary>
    int OrderIndex { get; }
    /// <summary>
    /// Determines whether the convertor can carry out the conversion
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    bool CanConvert(JsonElement element);

    /// <summary>
    /// Converts the <see cref="JsonElement"/> into a CLR type
    /// </summary>
    /// <returns></returns>
    object? Convert();
}
