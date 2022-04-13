using DiabetesManagement.Contracts;
using Microsoft.Extensions.Primitives;

namespace DiabetesManagement.Extensions.Extensions;

public static class RequestCollectionExtensions 
{
    public static TRequest Bind<TRequest>(this IEnumerable<KeyValuePair<string, StringValues>> requestCollection, IConvertorFactory convertorFactory)
    {
        var instance = Activator.CreateInstance<TRequest>();
        var requestType = typeof(TRequest);
        var requestDictionary = new Dictionary<string, StringValues>(requestCollection);
        foreach (var property in requestType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly))
        {
            if(requestDictionary.TryGetValue(property.Name, out var value))
            {
                if (property.CanWrite)
                {
                    var val = value.FirstOrDefault();
                    if(val == null)
                    {
                        continue;
                    }

                    var convertor = convertorFactory.GetConvertor(property.PropertyType, val);

                    if(convertor != null)
                    {   
                        property.SetValue(instance, convertor.Convert());
                    }

                    
                }
            }
        }

        return instance;
    }
}
