using Microsoft.Extensions.Primitives;

namespace DiabetesManagement.Extensions.Extensions;

public static class RequestCollectionExtensions 
{
    public static TRequest Bind<TRequest>(this IEnumerable<KeyValuePair<string, StringValues>> requestCollection)
    {
        var instance = Activator.CreateInstance<TRequest>();
        var requestType = typeof(TRequest);
        var requestDictionary = new Dictionary<string, StringValues>();
        foreach (var property in requestType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly))
        {
            if(requestDictionary.TryGetValue(property.Name, out var value))
            {
                if (property.CanWrite)
                {
                    property.SetValue(instance, value);
                }
            }
        }

        return instance;
    }
}
