using AutoMapper;
using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
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

        internal virtual async Task<IRequestHandler> GetRequestHandler(string queryOrCommand)
        {
            await Task.CompletedTask;
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

        internal async Task<IRequestHandler<TRequest>> GetRequestHandler<TRequest>(string queryOrCommand)
            where TRequest : IRequest
        {
            var requestHandler = await GetRequestHandler(queryOrCommand);
            return (IRequestHandler<TRequest>)requestHandler;
        }

        internal async Task<IRequestHandler<TRequest, TResponse>> GetRequestHandler<TRequest, TResponse>(string queryOrCommand)
            where TRequest : IRequest<TResponse>
        {
            var requestHandler = await GetRequestHandler(queryOrCommand);
            return (IRequestHandler<TRequest, TResponse>)requestHandler;
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

        public HandlerFactory(string connectionString, ILogger logger, IMapper mapper, IEnumerable<Assembly>? assemblies = null)
           : base(connectionString)
        {
            base.SetLogger = logger;
            Mapper = mapper;
            Init(assemblies ?? new[] { typeof(HandlerFactory).Assembly });
        }

        public HandlerFactory(ILogger logger, IDbConnection dbConnection, IMapper mapper, IDbTransaction? dbTransaction = null, IEnumerable<Assembly>? assemblies = null)
            : base(dbConnection, dbTransaction)
        {
            base.SetLogger = logger;
            Mapper = mapper;
            Init(assemblies ?? new[] { typeof(HandlerFactory).Assembly });
        }

        public IMapper Mapper { get; }

        public async Task Execute(string queryOrCommand, object request)
        {
            var requestHandler = await GetRequestHandler(queryOrCommand);
            await requestHandler.Handle(request);
        }

        public async Task Execute<TRequest>(string queryOrCommand, TRequest request)
            where TRequest : IRequest
        {
            var requestHandler = await GetRequestHandler<TRequest>(queryOrCommand);
            await requestHandler.Handle(request);
        }

        public async Task<TResponse> Execute<TRequest, TResponse>(string queryOrCommand, TRequest request)
            where TRequest : IRequest<TResponse>
        {
            var requestHandler = await GetRequestHandler<TRequest, TResponse>(queryOrCommand);
            return await requestHandler.Handle(request);
        }
    }
}
