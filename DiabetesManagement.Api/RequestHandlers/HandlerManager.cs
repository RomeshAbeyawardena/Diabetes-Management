using System;
using System.Data;

namespace DiabetesManagement.Api.RequestHandlers
{
    public class HandlerManager : Handler
    {
        public HandlerManager(IDbConnection dbConnection, IDbTransaction dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {

        }

        public HandlerManager(string connectionString)
            : base(connectionString)
        {

        }

        protected override void Dispose(bool disposing)
        {
            
        }
    }
}
