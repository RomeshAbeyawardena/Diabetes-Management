namespace DiabetesManagement.Shared.Contracts
{
    public interface IRequestHandler : IHandler
    {
        IHandlerFactory HandlerFactory { get; }
        IHandlerFactory SetHandlerFactory { set; }
        Task Handle(object request);
    }

    public interface IRequestHandler<TRequest> : IRequestHandler
    {
        Task Handle(TRequest request);
    }

    public interface IRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest>
    {
        new Task<TResponse> Handle(TRequest request);
    }
}
