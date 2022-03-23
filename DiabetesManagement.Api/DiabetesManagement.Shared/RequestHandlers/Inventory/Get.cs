using Dapper;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    public class Get : Handler
    {
        public Get(string connectionString)
           : base(connectionString)
        {

        }

        public Get(IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {
        }

        protected override void Dispose(bool disposing)
        {

        }

        public Task<Models.Inventory> GetInventory(Models.GetRequest request)
        {
            var finalSql = Queries.InventoryQuery
                .Replace("@@whereClause", Queries.GetInventoryWhereClause(request.InventoryId));

            TryOpenConnection();

            return DbConnection.QueryFirstOrDefaultAsync<Models.Inventory>(finalSql, new
            {
                id = request.InventoryId,
                key = request.Key,
                userId = request.UserId,
            }, GetOrBeginTransaction);
        }
    }
}
