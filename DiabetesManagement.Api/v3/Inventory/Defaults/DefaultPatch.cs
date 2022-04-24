using Inventory.Contracts;
using Inventory.Enumerations;

namespace Inventory.Defaults;

public class DefaultPatch<T> : IPatch<T>
{
    public PatchOperation Operation { get; set; }
    public string? Path { get; set; }
    public IDictionary<string, object> Value { get; set; }

    public T Apply(T entity)
    {
        throw new NotImplementedException();
    }
}
