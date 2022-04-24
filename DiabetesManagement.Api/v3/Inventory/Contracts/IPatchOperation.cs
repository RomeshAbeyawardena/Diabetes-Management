using Inventory.Enumerations;

namespace Inventory.Contracts;

public interface IPatchOperation : IPatchOperationWithValue
{
    new IDictionary<string, object> Value { get; set; } 
}

public interface IPatchOperationWithValue
{
    PatchOperation Operation { get; set; }
    string? Path { get; set; }
    object Value { get; set; }
}
