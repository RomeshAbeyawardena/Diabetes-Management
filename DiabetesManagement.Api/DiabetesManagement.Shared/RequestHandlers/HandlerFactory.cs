using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Contracts;
using System.Data;
using System.Reflection;

namespace DiabetesManagement.Shared.RequestHandlers
{
    public class HandlerFactory : HandlerBase, IHandlerFactory
    {
        private Dictionary<string, Type>? handlerDictionary;
        private Dictionary<string, IRequestHandler>? handlerTypes;

        private IRequestHandler GetRequestHandler(string queryOrCommand)
        {
            if (handlerTypes!.TryGetValue(queryOrCommand, out var requestHandler))
            {
                return requestHandler;
            }

            if(handlerDictionary!.TryGetValue(queryOrCommand, out var handlerType))
            {
                requestHandler = (IRequestHandler)Activator.CreateInstance(handlerType, DbConnection, GetOrBeginTransaction)!;
                requestHandler.SetHandlerFactory = this;
                handlerTypes.Add(queryOrCommand, requestHandler);
                return requestHandler;
            }

            throw new InvalidOperationException();
        }

        private bool IsHandler(Type type)
        {
            if(!type.GetInterfaces().Any(t => t == typeof(IRequestHandler)))
            {
                return false;
            }

            var t = type.GetCustomAttribute<HandlerDescriptorAttribute>();
            if(t == null)
            {
                return false;
            }
            
            handlerDictionary!.Add(t.QueryOrCommand, type);
            return true;
        }

        private void Init()
        {
            handlerDictionary = new();
            handlerTypes = new();
            typeof(HandlerFactory).Assembly.GetTypes().Where(IsHandler);
        }

        public HandlerFactory(string connectionString)
           : base(connectionString)
        {
            Init();
        }

        public HandlerFactory(IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {
            Init();
        }

        protected override void Dispose(bool disposing)
        {

        }

        public Task Execute(string queryOrCommand, object request)
        {
            return GetRequestHandler(queryOrCommand).Handle(request);
        }

        public Task Execute<TRequest>(string queryOrCommand, TRequest request)
        {
            var requestHandler = (IRequestHandler<TRequest>)GetRequestHandler(queryOrCommand).Handle(request!);
            return requestHandler.Handle(request);
        }

        public async Task<TResponse> Execute<TRequest, TResponse>(string queryOrCommand, TRequest request)
        {
            var requestHandler = (IRequestHandler<TRequest, TResponse>)GetRequestHandler(queryOrCommand).Handle(request!);
            return await requestHandler.Handle(request);
        }

        
    }
}
