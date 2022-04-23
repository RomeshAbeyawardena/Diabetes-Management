using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Humanizer;

namespace Inventory.Extensions;

using Inventory.Contracts;

public static class RequestCollectionExtensions
{
    public static TRequest Bind<TRequest>(this IEnumerable<KeyValuePair<string, StringValues>> requestCollection, IConvertorFactory convertorFactory)
        where TRequest : notnull
    {
        var instance = Activator.CreateInstance<TRequest>();
        var requestType = typeof(TRequest);
        var requestDictionary = new Dictionary<string, StringValues>(requestCollection);
        foreach (var property in requestType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            if (requestDictionary.TryGetValue(property.Name.Camelize(), out var value))
            {
                if (property.CanWrite)
                {
                    var val = value.FirstOrDefault();
                    if (val == null)
                    {
                        continue;
                    }

                    var convertor = convertorFactory.GetConvertor(property.PropertyType, val);

                    if (convertor != null)
                    {
                        property.SetValue(instance, convertor.Convert());
                    }
                }
            }
            else
            {
                var required = property.GetCustomAttribute<RequiredAttribute>();
                if (required != null)
                {
                    throw new ValidationException(required.ErrorMessage);
                }
            }
        }

        return instance;
    }
}
