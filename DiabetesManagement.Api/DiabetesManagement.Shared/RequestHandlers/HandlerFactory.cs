using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Contracts;
using System.Data;
using System.Reflection;

namespace DiabetesManagement.Shared.RequestHandlers
{
    public class HandlerFactory : Handler
    {
        private Dictionary<string, Type>? handlerDictionary;
        private Dictionary<string, IRequestHandler>? handlerTypes;
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

        public async Task<TResponse> Execute<TRequest, TResponse>(string queryOrCommand, TRequest request)
        {
            return (TResponse)(await Execute(queryOrCommand, request));
        }

        public Task Execute(string queryOrCommand, object request)
        {
            if(handlerTypes!.TryGetValue(queryOrCommand, out var requestHandler))
            {
                return requestHandler.Handle(request);
            }

            if(handlerDictionary!.TryGetValue(queryOrCommand, out var handlerType))
            {
                requestHandler = (IRequestHandler)Activator.CreateInstance(handlerType, DbConnection, GetOrBeginTransaction)!;
                handlerTypes.Add(queryOrCommand, requestHandler);
                return requestHandler.Handle(request);
            }

            throw new InvalidOperationException();
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
    }
}
