using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
using DiabetesManagement.Shared.Extensions;
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
            
        }

        protected override async Task<Models.InventoryHistory> HandleAsync(GetRequest request)
        {
            var inventoryHistory = new Models.InventoryHistory();

            string? orderBy = default;
            if (request.IsLatest)
            {
                orderBy = "ORDER BY [VERSION] DESC";
            }

            var inventoryHistoryItems = await inventoryHistory.Get(DbConnection, request,
                orderByQuery: orderBy!,
                builder: builder => builder.Add<Models.InventoryHistory, Models.Inventory>(p => p.InventoryId, c => c.InventoryId),
                transaction: GetOrBeginTransaction);

            TryOpenConnection();

            return inventoryHistoryItems.FirstOrDefault()!;
        }
    }
}
