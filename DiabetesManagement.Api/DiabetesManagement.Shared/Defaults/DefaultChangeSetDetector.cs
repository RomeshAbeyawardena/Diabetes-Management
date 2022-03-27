using DiabetesManagement.Shared.Contracts;
using DiabetesManagement.Shared.Extensions;
using System.Reflection;

namespace DiabetesManagement.Shared.Defaults
{
    public class DefaultChangeSetDetector : IChangeSetDetector
    {
        public IChangeSet DetectChanges<TSource, TDestination>(TSource source, TDestination destination)
        {
            static IDictionary<string, PropertyInfo> GetProperties(Type type)
            {
                return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).ToDictionary(k => k.Name, v => v);
            }

            var changeSet = new DefaultChangeSet<TSource, TDestination>(source, destination);

            changeSet.SourceProperties.Copy(GetProperties(typeof(TSource)));
            changeSet.DestinationProperties.Copy(GetProperties(typeof(TDestination)));
            
            foreach(var (name, property) in changeSet.SourceProperties)
            {
                var sourceValue = property.GetValue(source);

                if (sourceValue != null && changeSet.DestinationProperties.TryGetValue(name, out var destinationProperty))
                {
                   var destinationValue = destinationProperty.GetValue(destination);

                    if (destinationValue != null && !destinationValue.Equals(sourceValue))
                    {
                        changeSet.ChangedProperties.Add(property, destinationProperty);
                    }
                }
            }

            return changeSet;
        }
    }
}
