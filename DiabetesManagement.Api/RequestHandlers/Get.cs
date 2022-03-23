using Dapper;
using DiabetesManagement.Api.Models;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers
{
    public class Get : Handler
    {
        public Get(string connectionString)
            : base(connectionString)
        {

        }

        public Get(IDbConnection dbConnection, IDbTransaction dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {
        }

        protected override void Dispose(bool disposing)
        {
            
        }

        public Task<Inventory> GetInventory(GetRequest request)
        {
            var finalSql = Queries.InventoryQuery
                .Replace("@@whereClause", Queries.GetInventoryWhereClause(request.InventoryId));

            TryOpenConnection();

            return DbConnection.QueryFirstOrDefaultAsync<Inventory>(finalSql, new
            {
                id = request.InventoryId,
                key = request.Key,
                userId = request.UserId,
            }, GetOrBeginTransaction);
        }

        public Task<InventoryHistory> GetInventoryHistory(GetRequest request)
        {
            var finalSql = Queries.InventoryHistoryQuery
                .Replace("@@whereClause", Queries.GetInventoryHistoryWhereClause(request.Version));

            TryOpenConnection();

            return DbConnection.QueryFirstOrDefaultAsync<InventoryHistory>(finalSql, new { 
                key = request.Key, 
                userId = request.UserId, 
                type = request.Type,
                version = request.Version }, GetOrBeginTransaction);
        }
    }
}
