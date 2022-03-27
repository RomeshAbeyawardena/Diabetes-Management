using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
using DiabetesManagement.Shared.Extensions;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    [HandlerDescriptor(Commands.UpdateInventory)]
    public class Put : HandlerBase<SaveCommand, Guid>
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

        protected async override Task<Guid> HandleAsync(SaveCommand request)
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

            Models.InventoryHistory inventoryHistory = new();

            var putRequest = new PutRequest();

            bool hasChanges = false;

            if(request.Inventory!.DefaultType != inventoryRecord.DefaultType)
            {
                putRequest.DefaultType = request.Inventory.DefaultType;
                hasChanges = true;
            }

            if (request.Inventory.DefaultType != inventoryRecord.DefaultType)
            {
                putRequest.DefaultType = request.Inventory.DefaultType;
                hasChanges = true;
            }

            if (request.Inventory.DefaultType != inventoryRecord.DefaultType)
            {
                putRequest.DefaultType = request.Inventory.DefaultType;
                hasChanges = true;
            }

            if(hasChanges != true){
                return inventoryRecord.InventoryId;
            }

            var result = await inventoryHistory.Update(putRequest, DbConnection, transaction);
            
            if (request.CommitOnCompletion)
            {
                transaction.Commit();
            }

            return result;
        }
    }
}
