using Inventory.Contracts;
using Inventory.Enumerations;

namespace Inventory.Defaults;

public class DefaultPatchOperation : IPatchOperation
{
    public DefaultPatchOperation()
    {
        Value = new Dictionary<string, object>();
    }

    public PatchOperation Operation { get; set; }
    public string? Path { get; set; }
    public IDictionary<string, object> Value { get; set; }
    object IPatchOperationWithValue.Value { 
        get => Value;
        set => Value = (IDictionary<string, object>)value; 
    }
}
