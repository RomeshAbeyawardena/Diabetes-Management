using Inventory.Enumerations;

namespace Inventory.Contracts;

public interface IPatch<T>
{
    PatchOperation Operation { get; set; }
    string? Path { get; set; }
    IDictionary<string, object> Value { get; set; }
    T Apply(T entity);
}
