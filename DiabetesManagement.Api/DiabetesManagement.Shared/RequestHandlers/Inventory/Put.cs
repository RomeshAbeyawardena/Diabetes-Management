using Dapper;
using DiabetesManagement.Shared.Attributes;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    [HandlerDescriptor(Commands.UpdateInventory)]
    public class Put : HandlerBase<SaveRequest, Guid>
    {
        protected override void Dispose(bool disposing)
        {

        }

        public Put(string connectionString) : base(connectionString)
        {
        }

        public Put(IDbConnection dbConnection, IDbTransaction? dbTransaction = null) 
            : base(dbConnection, dbTransaction)
        {
        }

        protected async override Task<Guid> HandleAsync(SaveRequest request)
        {
            var transaction = GetOrBeginTransaction;
            var inventory = request.Inventory;
            var getRequest = new GetRequest
            {
                InventoryId = inventory?.InventoryId == default ? null : inventory?.InventoryId,
                Key = inventory!.Key,
                UserId = inventory.UserId,
            };

            getRequest.InventoryId = inventory.InventoryId == default ? null : inventory.InventoryId;

            var inventoryRecord = await HandlerFactory!.Execute<GetRequest, Models.Inventory>(Queries.GetInventory, getRequest);

            if (inventoryRecord == null)
            {
                throw new DataException("Inventory record not found");
            }

            var result = await DbConnection.ExecuteScalarAsync<Guid>(Commands.UpdateInventoryCommand, new
            {
                key = inventory.Key,
                defaultType = inventory.DefaultType,
                modified = inventory.Modified,
                inventoryRecord.InventoryId
            }, transaction);

            if (request.CommitOnCompletion)
            {
                transaction.Commit();
            }

            return result;
        }
    }
}
