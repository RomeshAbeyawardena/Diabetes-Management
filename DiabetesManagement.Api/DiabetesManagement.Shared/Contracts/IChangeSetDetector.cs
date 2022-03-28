namespace DiabetesManagement.Shared.Contracts
{
    public interface IChangeSetDetector
    {
        IChangeSet<TSource, TDestination> DetectChanges<TSource, TDestination>(TSource source, TDestination destination);
    }
}
