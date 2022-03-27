namespace DiabetesManagement.Shared.Contracts
{
    public interface IChangeSetDetector
    {
        IChangeSet DetectChanges<TSource, TDestination>(TSource source, TDestination destination);
    }
}
