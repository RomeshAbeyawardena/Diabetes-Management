namespace DiabetesManagement.Shared.Contracts
{
    public interface IHandlerFactory : IDisposable
    {
        Task Execute(string queryOrCommand, object request);
        Task Execute<TRequest>(string queryOrCommand, TRequest request);
        Task<TResponse> Execute<TRequest, TResponse>(string queryOrCommand, TRequest request);
    }
}
