using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
using DiabetesManagement.Shared.Extensions;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    [HandlerDescriptor(Queries.ListInventoryHistory)]
    public class List : HandlerBase<ListRequest, IEnumerable<Models.InventoryHistory>>
    {
        public List(string connectionString) : base(connectionString)
        {
        }

        public List(IDbConnection dbConnection, IDbTransaction? dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        protected override void Dispose(bool disposing)
        {

        }

        protected override async Task<IEnumerable<Models.InventoryHistory>> HandleAsync(ListRequest request)
        {
            var inventoryModel = new Models.InventoryHistory();
            TryOpenConnection();

            var orderBy = "ORDER BY [VERSION] DESC";

            return await inventoryModel
                .Get(DbConnection, request,
                orderByQuery: orderBy!,
                builder: builder => builder.Add<Models.InventoryHistory,
                    Models.Inventory>(p => p.InventoryId, c => c.InventoryId),
                transaction: GetOrBeginTransaction, topAmount: 20);
        }
    }
}
