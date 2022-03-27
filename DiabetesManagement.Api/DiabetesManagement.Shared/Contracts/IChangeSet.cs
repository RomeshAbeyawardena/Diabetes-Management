using System.Reflection;

namespace DiabetesManagement.Shared.Contracts
{
    public interface IChangeSet
    {
        IDictionary<string, PropertyInfo> SourceProperties { get; }
        IDictionary<string, PropertyInfo> DestinationProperties { get; }
        IDictionary<PropertyInfo, PropertyInfo> ChangedProperties { get; }
        bool HasChanges { get; }

        object CommitChanges(object source);
    }

    public interface IChangeSet<TSource, TDestination> : IChangeSet
    {
        TSource Source { get; }
        TDestination Destination { get; }
        TDestination CommitChanges(TSource source);
    }
}
