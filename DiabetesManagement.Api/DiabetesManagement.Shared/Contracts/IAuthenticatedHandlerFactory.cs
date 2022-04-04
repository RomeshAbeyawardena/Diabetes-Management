namespace DiabetesManagement.Shared.Contracts
{
    public interface IAuthenticatedHandlerFactory : IHandlerFactory
    {
        Task BypassAuthentication(Func<IAuthenticatedHandlerFactory, Task> action);
        Task<TResult> BypassAuthentication<TResult>(Func<IAuthenticatedHandlerFactory, Task<TResult>> action);

        Task<bool> IsAuthenticated(string key, string secret);
        Task<bool> IsAuthenticated(Models.ApiToken apiToken);
    }
}
