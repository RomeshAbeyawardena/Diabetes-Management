﻿using Dapper;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    public class Get : HandlerBase<GetRequest, Models.InventoryHistory>
    {
        public Get(string connectionString) : base(connectionString)
        {
        }

        public Get(IDbConnection dbConnection, IDbTransaction? dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }

        protected override Task<Models.InventoryHistory> HandleAsync(GetRequest request)
        {
            var finalSql = Queries.InventoryHistoryQuery
                .Replace("@@whereClause", Queries.GetInventoryHistoryWhereClause(request.Version));

            TryOpenConnection();

            return DbConnection.QueryFirstOrDefaultAsync<Models.InventoryHistory>(finalSql, new
            {
                key = request.Key,
                userId = request.UserId,
                type = request.Type,
                version = request.Version
            }, GetOrBeginTransaction);
        }
    }
}
