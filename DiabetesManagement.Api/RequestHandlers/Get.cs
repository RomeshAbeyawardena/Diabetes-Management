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
            var sql = @"SELECT TOP(1) [I].[INVENTORYID], [I].[KEY], [I].[USERID],
                    [I].[CREATED], [I].[MODIFIED] FROM [dbo].[INVENTORY][I]";

            var whereClause = "WHERE [I].[KEY] = @key AND [I].[USERID] = @userId ";

            var finalSql = sql
                .Replace("@@whereClause", whereClause);

            TryOpenConnection();

            return DbConnection.QueryFirstOrDefaultAsync<Inventory>(finalSql, new
            {
                key = request.Key,
                userId = request.UserId,
            }, GetOrBeginTransaction);
        }

        public Task<InventoryHistory> GetInventoryHistory(GetRequest request)
        {
            var sql = @"SELECT TOP(1) [I].[INVENTORYID], [I].[KEY], [I].[USERID],
                    [I].[CREATED], [I].[MODIFIED],
                    [IH].[INVENTORY_HISTORYID], [IH].[Version],
                    [IH].[Items], [IH].[Created] [InventoryHistoryCreated]
                FROM [dbo].[INVENTORY_HISTORY] [IH]
                INNER JOIN [dbo].[INVENTORY][I]
                ON [IH].[INVENTORYID] = [I].[INVENTORYID]
                    @@whereClause
                ORDER BY [VERSION] DESC";

            var whereClause = "WHERE [I].[KEY] = @key AND [I].[USERID] = @userId ";

            if (request.Version.HasValue)
            {
                whereClause += " AND [IH].[Version] = @version";
            }

            var finalSql = sql
                .Replace("@@whereClause", whereClause);

            TryOpenConnection();

            return DbConnection.QueryFirstOrDefaultAsync<InventoryHistory>(finalSql, new { 
                key = request.Key, 
                userId = request.UserId, 
                version = request.Version }, GetOrBeginTransaction);
        }
    }
}
