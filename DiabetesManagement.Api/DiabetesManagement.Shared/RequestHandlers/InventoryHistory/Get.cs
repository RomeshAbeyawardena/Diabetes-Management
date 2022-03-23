using Dapper;

namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    public class Get : Handler
    {
        public Task<Models.InventoryHistory> GetInventoryHistory(Models.GetRequest request)
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
