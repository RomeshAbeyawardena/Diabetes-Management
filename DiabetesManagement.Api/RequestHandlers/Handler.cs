using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DiabetesManagement.Api.RequestHandlers
{
    public abstract class Handler : IDisposable
    {
        void IDisposable.Dispose()
        {
            dbTransaction?.Dispose();
            dbTransaction = null;
            dbConnection?.Dispose();
            dbConnection = null;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private IDbConnection dbConnection;
        private IDbTransaction dbTransaction;

        protected abstract void Dispose(bool disposing);
        protected IDbTransaction GetOrBeginTransaction => dbTransaction ??= dbConnection.BeginTransaction();
        protected IDbConnection DbConnection => dbConnection;
        
        public IDbTransaction UseTransaction { set => dbTransaction = value; }
        
        protected bool TryOpenConnection()
        {
            if(dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
                return true;
            }

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
        }


    }
}
