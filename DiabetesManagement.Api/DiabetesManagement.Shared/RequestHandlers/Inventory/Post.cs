using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
using DiabetesManagement.Shared.Extensions;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    [HandlerDescriptor(Commands.SaveInventory, Permissions.Add, Permissions.Edit)]
    public class Post : HandlerBase<SaveCommand, Guid>
    {
        protected override void Dispose(bool disposing)
        {

        }

        /// <summary>
        /// Initialises a new instance of the POST handler to initiate a new connection
        /// </summary>
        /// <param name="connectionString">The SQL connection string to be used to intialise the database connection</param>
        public Post(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Initialises a new instance of the POST handler to reuse an existing connection and optional existing transaction objects
        /// </summary>
        /// <param name="dbConnection">An existing connection</param>
        /// <param name="dbTransaction">Optional existing transaction</param>
        public Post(IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {

        }

        protected override async Task<Guid> HandleAsync(SaveCommand request)
        {
            var transaction = GetOrBeginTransaction;
            var inventory = request.Inventory;

            var getRequest = new GetRequest
            {
                InventoryId = inventory?.InventoryId == default ? null : inventory?.InventoryId,
                Key = inventory!.Key,
                UserId = inventory.UserId       
            };

            Logger.LogInformation("Checking INVENTORY exists...");
            var inventoryRecord = await this.HandlerFactory!.Execute<GetRequest, Models.Inventory>(Queries.GetInventory, getRequest);

            if(inventoryRecord == null)
            {
                Logger.LogInformation("Saving INVENTORY...");

                if(inventory.InventoryId == default)
                {
                    inventory.InventoryId = Guid.NewGuid();
                }

                if(inventory.Created == default)
                {
                    inventory.Created = DateTimeOffset.UtcNow;
                }

                inventoryRecord = new();

                var result = await inventoryRecord.Insert(DbConnection, GetOrBeginTransaction);

                if (request.CommitOnCompletion)
                {
                    Logger.LogInformation("Committing changes...");
                    transaction.Commit();
                }

                return result;
            }

            if (request.ThrowIfInventoryRowExists)
            {
                throw new DataException("Inventory record already exists");
            }

            return inventoryRecord.InventoryId;
        }
    }
}
