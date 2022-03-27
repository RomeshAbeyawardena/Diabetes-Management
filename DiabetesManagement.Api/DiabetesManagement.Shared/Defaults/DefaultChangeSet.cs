using DiabetesManagement.Shared.Contracts;
using System.Reflection;

namespace DiabetesManagement.Shared.Defaults
{
    public class DefaultChangeSet<TSource, TDestination> : IChangeSet<TSource, TDestination>
    {
        public DefaultChangeSet(TSource source, TDestination destination)
        {
            SourceProperties = new Dictionary<string, PropertyInfo>();
            DestinationProperties = new Dictionary<string, PropertyInfo>();
            ChangedProperties = new Dictionary<PropertyInfo, PropertyInfo>();
            Source = source;
            Destination = destination;
        }

        public IDictionary<string, PropertyInfo> SourceProperties { get; }

        public IDictionary<string, PropertyInfo> DestinationProperties { get; }

        public IDictionary<PropertyInfo, PropertyInfo> ChangedProperties { get; }

        public bool HasChanges { get; private set; }
        public TSource Source { get; }
        public TDestination Destination { get; }

        public TDestination CommitChanges(TSource source)
        {
            var destination = Activator.CreateInstance<TDestination>();
            foreach(var (sourceProperty, destinationProperty) in ChangedProperties)
            {
                var value = sourceProperty.GetValue(source);
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
