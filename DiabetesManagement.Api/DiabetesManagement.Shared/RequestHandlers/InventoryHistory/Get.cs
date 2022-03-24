using Dapper;
using DiabetesManagement.Shared.Attributes;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    [HandlerDescriptor(Queries.GetInventoryHistory, Permissions.View)]
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
                .Replace("@@whereClause", Queries.GetInventoryHistoryWhereClause(request));

            TryOpenConnection();

            return DbConnection.QueryFirstOrDefaultAsync<Models.InventoryHistory>(finalSql, new
            {
                inventoryHistoryId = request.InventoryHistoryId,
                key = request.Key,
                userId = request.UserId,
                type = request.Type,
                version = request.Version
            }, GetOrBeginTransaction);
        }
    }
}
