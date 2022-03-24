using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Contracts;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;

namespace DiabetesManagement.Shared.RequestHandlers
{
    public class HandlerFactory : HandlerBase, IHandlerFactory
    {
        private Dictionary<string, Type>? handlerDictionary;
        private Dictionary<string, IRequestHandler>? handlerTypes;
        private readonly ILogger logger;

        internal IRequestHandler GetRequestHandler(string queryOrCommand)
        {
            if (handlerTypes!.TryGetValue(queryOrCommand, out var requestHandler))
            {
                return requestHandler;
            }

            if(handlerDictionary!.TryGetValue(queryOrCommand, out var handlerType))
            {
                requestHandler = (IRequestHandler)Activator.CreateInstance(handlerType, DbConnection, GetOrBeginTransaction)!;
                requestHandler.SetHandlerFactory = this;
                requestHandler.SetLogger = logger;
                handlerTypes.Add(queryOrCommand, requestHandler);
                return requestHandler;
            }

            throw new InvalidOperationException();
        }

        private bool AddIfIsHandler(Type type)
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
            foreach(var type in typeof(HandlerFactory).Assembly.GetTypes())
            {
                if (AddIfIsHandler(type))
                {
                    logger.LogInformation("Adding {type}", type);
                }
            }
        }

        public HandlerFactory(string connectionString, ILogger logger)
           : base(connectionString)
        {
            Init();
            this.logger = logger;
        }

        public HandlerFactory(ILogger logger, IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {
            this.logger = logger;
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
