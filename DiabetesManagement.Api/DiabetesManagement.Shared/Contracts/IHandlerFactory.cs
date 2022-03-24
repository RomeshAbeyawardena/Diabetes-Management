namespace DiabetesManagement.Shared.Contracts
{
    public interface IHandlerFactory : IDisposable
    {
        Task Execute(string queryOrCommand, object request);
        Task Execute<TRequest>(string queryOrCommand, TRequest request)
            where TRequest : IRequest;
        Task<TResponse> Execute<TRequest, TResponse>(string queryOrCommand, TRequest request)
            where TRequest : IRequest<TResponse>;
    }
}
