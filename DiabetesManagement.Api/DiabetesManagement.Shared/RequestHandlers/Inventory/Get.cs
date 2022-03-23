using Dapper;
using DiabetesManagement.Shared.Attributes;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    [HandlerDescriptor(Queries.GetInventory)]
    public class Get : HandlerBase<GetRequest, Models.Inventory>
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

        protected override Task<Models.Inventory> HandleAsync(GetRequest request)
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
