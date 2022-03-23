using System;
using System.Data;

namespace DiabetesManagement.Api.RequestHandlers
{
    public class HandlerManager : Handler
    {
        private Get getHandler;
        private Post postHandler;

        protected override void Dispose(bool disposing)
        {
            
        }

        public Get GetHandler => getHandler ??= new Get(DbConnection);
        public Post PostHandler => postHandler ??= new Post(DbConnection);

        public HandlerManager(IDbConnection dbConnection, IDbTransaction dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {

        }

        public HandlerManager(string connectionString)
            : base(connectionString)
        {

        }

    }
}
