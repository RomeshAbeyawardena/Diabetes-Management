using DiabetesManagement.Shared.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reactive.Subjects;

namespace DiabetesManagement.Shared.RequestHandlers
{
    public abstract class HandlerBase<TRequest> : HandlerBase, IRequestHandler<TRequest>
        where TRequest: IRequest
    {
        IHandlerFactory IRequestHandler.HandlerFactory => HandlerFactory!;

        protected IHandlerFactory? HandlerFactory { get; private set; }

        public IHandlerFactory SetHandlerFactory { set => HandlerFactory = value; }

        public HandlerBase(string connectionString)
           : base(connectionString)
        {

        }

        public HandlerBase(IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {
        }

        Task IRequestHandler.Handle(object request)
        {
            return Handle((TRequest)request);
        }

        public abstract Task Handle(TRequest request);
    }

    public abstract class HandlerBase<TRequest, TResponse> : HandlerBase<TRequest>, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        protected abstract Task<TResponse> HandleAsync(TRequest request);

        public HandlerBase(string connectionString)
           : base(connectionString)
        {

        }

        public HandlerBase(IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {
        }

        public override Task Handle(TRequest request)
        {
            return HandleAsync(request);
        }

        Task<TResponse> IRequestHandler<TRequest, TResponse>.Handle(TRequest request)
        {
            return HandleAsync(request);
        }
    }

    public abstract class HandlerBase : IHandler
    {
        void IDisposable.Dispose()
        {
            foreach (var subscriber in subscribers)
            {
                subscriber?.Dispose();
            }

            dbTransaction?.Dispose();
            dbTransaction = null;

            logger!.LogInformation("Db transaction disposed");
            dbConnection?.Dispose();
            dbConnection = null;
            logger!.LogInformation("Db connection disposed");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private readonly List<IDisposable> subscribers;
        private ILogger? logger;
        private IDbConnection? dbConnection;
        private IDbTransaction? dbTransaction;
        private readonly ISubject<ILogger> loggerSubject;
        private readonly ISubject<IDbTransaction> transactionSubject;

        protected abstract void Dispose(bool disposing);
        
        protected IDbTransaction GetOrBeginTransaction
        {
            get
            {
                if (dbTransaction == null)
                {
                    TryOpenConnection();
                    SetTransaction = dbConnection!.BeginTransaction();
                }

                return dbTransaction!;
            }
        }
        protected IDbConnection DbConnection => dbConnection!;
        protected ILogger Logger => logger!;
        
        protected void OnLoggerSet(Action<ILogger> onNext)
        {
            subscribers.Add(loggerSubject.Subscribe(onNext));
        }

        protected void OnTransactionSet(Action<IDbTransaction> onNext)
        {
            subscribers.Add(transactionSubject.Subscribe(onNext));
        }

        protected bool TryOpenConnection()
        {
            if(dbConnection!.State == ConnectionState.Closed)
            {
                logger!.LogInformation("Opening db connection");
                dbConnection.Open();
                return true;
            }

            logger!.LogInformation("Db connection already opened");
            return false;
        }

        public HandlerBase(string connectionString)
            : this(new SqlConnection(connectionString))
        {

        }

        public HandlerBase(IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
        {
            this.dbConnection = dbConnection;
            this.dbTransaction = dbTransaction;
            loggerSubject = new Subject<ILogger>();
            transactionSubject = new Subject<IDbTransaction>();
            subscribers = new List<IDisposable>();
        }

        public IObservable<ILogger> LoggerChanged => loggerSubject;
        public IObservable<IDbTransaction> DbTransactionChanged => transactionSubject;

        public IDbTransaction SetTransaction { 
            set 
            { 
                transactionSubject.OnNext(value); 
                dbTransaction = value; 
            } 
        }

        public ILogger SetLogger { 
            set 
            { 
                loggerSubject.OnNext(value);
                logger = value; 
            }
        }
    }
}
