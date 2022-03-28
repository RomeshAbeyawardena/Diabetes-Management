using DiabetesManagement.Shared.Contracts;
using DiabetesManagement.Shared.Extensions;
using System.Reflection;

namespace DiabetesManagement.Shared.Defaults
{
    public class DefaultChangeSetDetector : IChangeSetDetector
    {
        private IEnumerable<PropertyInfo>? sourceProperties;

        public DefaultChangeSetDetector()
        {

        }

        public DefaultChangeSetDetector(IEnumerable<PropertyInfo> sourceProperties)
        {
            this.sourceProperties = sourceProperties;
        }

        public IChangeSet<TSource, TDestination> DetectChanges<TSource, TDestination>(TSource source, TDestination destination)
        {
            IDictionary<string, PropertyInfo> GetProperties(Type type)
            {
                return (sourceProperties ??= type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)).ToDictionary(k => k.Name, v => v);
            }

            var changeSet = new DefaultChangeSet<TSource, TDestination>(this, source, destination);

            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);

            if (sourceType == destinationType)
            {
                var properties = GetProperties(sourceType);
                changeSet.SourceProperties.Copy(properties);
                changeSet.DestinationProperties.Copy(properties);
            }
            else
            {
                changeSet.SourceProperties.Copy(GetProperties(sourceType));
                changeSet.DestinationProperties.Copy(GetProperties(destinationType));
            }

            foreach(var (name, property) in changeSet.SourceProperties)
            {
                var sourceValue = property.GetValue(source);

                if (changeSet.DestinationProperties.TryGetValue(name, out var destinationProperty))
                {
                   var destinationValue = destinationProperty.GetValue(destination);

                    if (sourceValue == null && destinationValue == null)
                    {
                        continue;
                    }

                    if (sourceValue != null && destinationValue != null && (sourceValue.Equals(destinationValue) || sourceValue.Equals(sourceValue.GetDefaultValue())))
                    {
                        continue;
                    }
                    else
                    {
                        changeSet.ChangedProperties.Add(property, destinationProperty);
                    }
                }
            }

            return changeSet;
        }
    }
}
