using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Reactive.Subjects;

namespace DiabetesManagement.Api.RequestHandlers
{
    public abstract class Handler : IDisposable
    {
        void IDisposable.Dispose()
        {
            subscriber?.Dispose();
            dbTransaction?.Dispose();
            dbTransaction = null;

            logger.LogInformation("Db transaction disposed");
            dbConnection?.Dispose();
            dbConnection = null;
            logger.LogInformation("Db connection disposed");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private IDisposable subscriber;
        private ILogger logger;
        private IDbConnection dbConnection;
        private IDbTransaction dbTransaction;
        private readonly ISubject<ILogger> loggerSubject;
        
        protected abstract void Dispose(bool disposing);
        protected IDbTransaction GetOrBeginTransaction => dbTransaction ??= dbConnection.BeginTransaction();
        protected IDbConnection DbConnection => dbConnection;
        protected ILogger Logger => logger;
        protected IObservable<ILogger> LoggerSubject => loggerSubject;
        
        protected void OnLoggerSet(Action<ILogger> onNext)
        {
            if(subscriber != null)
            {
                subscriber.Dispose();
            }

            subscriber = loggerSubject.Subscribe(onNext);
        }
        
        protected bool TryOpenConnection()
        {
            if(dbConnection.State == ConnectionState.Closed)
            {
                logger.LogInformation("Opening db connection");
                dbConnection.Open();
                return true;
            }

            logger.LogInformation("Db connection already opened");
            return false;
        }

        public Handler(string connectionString)
            : this(new SqlConnection(connectionString))
        {

        }

        public Handler(IDbConnection dbConnection, IDbTransaction dbTransaction = null)
        {
            this.dbConnection = dbConnection;
            this.dbTransaction = dbTransaction;
            loggerSubject = new Subject<ILogger>();
        }

        public IDbTransaction UseTransaction { set => dbTransaction = value; }

        public ILogger SetLogger { 
            set 
            { 
                loggerSubject.OnNext(value);
                logger = value; 
            }
        }
    }
}
