using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Contracts;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;

namespace DiabetesManagement.Shared.RequestHandlers
{
    public class HandlerFactory : HandlerBase, IHandlerFactory
    {
        public static IEnumerable<Assembly> GetAssemblies(params Assembly[] assemblies)
        {
            return assemblies.Append(typeof(HandlerFactory).Assembly);
        }

        private Dictionary<string, Type>? handlerDictionary;
        private Dictionary<string, IRequestHandler>? handlerTypes;

        internal IRequestHandler GetRequestHandler(string queryOrCommand)
        {
            if (handlerTypes!.TryGetValue(queryOrCommand, out var requestHandler))
            {
                return requestHandler;
            }

            if(handlerDictionary!.TryGetValue(queryOrCommand, out var handlerType))
            {
                requestHandler = (IRequestHandler)Activator.CreateInstance(handlerType, DbConnection, GetOrBeginTransaction)!;
                requestHandler.SetLogger = Logger;
                requestHandler.SetHandlerFactory = this;
                handlerTypes.Add(queryOrCommand, requestHandler);
                return requestHandler;
            }

            throw new InvalidOperationException();
        }

        internal IRequestHandler<TRequest> GetRequestHandler<TRequest>(string queryOrCommand)
            where TRequest : IRequest
        {
            return (IRequestHandler<TRequest>)GetRequestHandler(queryOrCommand);
        }

        internal IRequestHandler<TRequest, TResponse> GetRequestHandler<TRequest, TResponse>(string queryOrCommand)
            where TRequest : IRequest<TResponse>
        {
            return (IRequestHandler<TRequest, TResponse>)GetRequestHandler(queryOrCommand);
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

        private void Init(IEnumerable<Assembly> assemblies)
        {
            handlerDictionary = new();
            handlerTypes = new();

            var types = assemblies.SelectMany(a => a.GetTypes());
            foreach (var type in types)
            {
                if (AddIfIsHandler(type))
                {
                    Logger.LogInformation("Adding {type}", type);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {

        }

        public HandlerFactory(string connectionString, ILogger logger, IEnumerable<Assembly>? assemblies = null)
           : base(connectionString)
        {
            base.SetLogger = logger;
            Init(assemblies ?? new[] { typeof(HandlerFactory).Assembly });
        }

        public HandlerFactory(ILogger logger, IDbConnection dbConnection, IDbTransaction? dbTransaction = null, IEnumerable<Assembly>? assemblies = null)
            : base(dbConnection, dbTransaction)
        {
            base.SetLogger = logger;
            Init(assemblies ?? new[] { typeof(HandlerFactory).Assembly });
        }

        public Task Execute(string queryOrCommand, object request)
        {
            return GetRequestHandler(queryOrCommand).Handle(request);
        }

        public Task Execute<TRequest>(string queryOrCommand, TRequest request)
            where TRequest : IRequest
        {
            var requestHandler = GetRequestHandler<TRequest>(queryOrCommand);
            return requestHandler.Handle(request);
        }

        public async Task<TResponse> Execute<TRequest, TResponse>(string queryOrCommand, TRequest request)
            where TRequest : IRequest<TResponse>
        {
            var requestHandler = GetRequestHandler<TRequest, TResponse>(queryOrCommand);
            return await requestHandler.Handle(request);
        }
    }
}
