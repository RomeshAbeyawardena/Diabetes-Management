using DiabetesManagement.Shared.Contracts;
using DiabetesManagement.Shared.Extensions;
using System.Reflection;

namespace DiabetesManagement.Shared.Defaults
{
    public class DefaultChangeSet<TSource, TDestination> : IChangeSet<TSource, TDestination>
    {
        public DefaultChangeSet(IChangeSetDetector changeSetDetector, TSource source, TDestination destination)
        {
            SourceProperties = new Dictionary<string, PropertyInfo>();
            DestinationProperties = new Dictionary<string, PropertyInfo>();
            ChangedProperties = new Dictionary<PropertyInfo, PropertyInfo>();
            ChangeSetDetector = changeSetDetector;
            Source = source;
            Destination = destination;
        }

        public IDictionary<string, PropertyInfo> SourceProperties { get; }

        public IDictionary<string, PropertyInfo> DestinationProperties { get; }

        public IDictionary<PropertyInfo, PropertyInfo> ChangedProperties { get; }

        public bool HasChanges => ChangedProperties.Any();
        public TSource Source { get; }
        public TDestination Destination { get; }

        public IChangeSetDetector ChangeSetDetector { get; }

        public TDestination CommitChanges(TSource source)
        {
            var destination = Activator.CreateInstance<TDestination>();
            foreach(var (sourceProperty, destinationProperty) in ChangedProperties)
            {
                var value = sourceProperty.GetValue(source);
                var defaultValue = sourceProperty.PropertyType.GetDefaultValue();
                if(value == null || value.Equals(defaultValue))
                {
                    continue;
                }

                destinationProperty.SetValue(destination, value);
            }

            return destination;
        }

        public object CommitChanges(object source)
        {
            return CommitChanges((TSource)source)!;
        }
    }
}
