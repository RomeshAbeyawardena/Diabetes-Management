using Inventory.Contracts;

namespace Inventory.Core.Defaults;

public class DefaultPatcher : IPatcher
{
    private T ApplyAdd<T>(IEnumerable<IPatchOperation> operations)
    {
        var modelType = typeof(T);
        var modelProperties = modelType.GetProperties();
        var originalItem = operations.FirstOrDefault();
        var model = Activator.CreateInstance(modelType);
        for(var index=1; index <= operations.Count(); index++)
        {
            var operation = operations.ElementAt(index);
            if(operation == null)
            {
                continue;
            }

            var prop = modelProperties.FirstOrDefault(p => p.Name == operation.Path.TrimStart('/'));

            if(prop == null)
            {
                throw new NullReferenceException();
            }

            prop.SetValue(model, operation.Value);
        }

        return (T)model;
    }

    public T Apply<T>(IEnumerable<IPatchOperation> operations)
    {
        var result = ApplyAdd<T>(operations.Where(op => op.Operation == Enumerations.PatchOperation.Add));

        return result;
    }
}
