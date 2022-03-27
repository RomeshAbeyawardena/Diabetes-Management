using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
using DiabetesManagement.Shared.Extensions;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    [HandlerDescriptor(Queries.GetInventory, Permissions.View)]
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

        protected override async Task<Models.Inventory> HandleAsync(GetRequest request)
        {
            var inventoryModel = new Models.Inventory();
            TryOpenConnection();
            var result = await inventoryModel.Get(DbConnection, request, transaction: GetOrBeginTransaction);

            return result.FirstOrDefault()!;
        }

    }
}
