using Dapper;
using DiabetesManagement.Shared.Attributes;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    [HandlerDescriptor(Commands.SaveInventory)]
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

                var result = await DbConnection.ExecuteScalarAsync<Guid>(Commands.InsertInventoryCommand, new
                {
                    inventoryId = inventory.InventoryId == default ? Guid.NewGuid() : inventory.InventoryId,
                    key = inventory.Key,
                    userId = inventory.UserId,
                    defaultType = inventory.DefaultType,
                    created = inventory.Created == default ? DateTimeOffset.UtcNow : inventory.Created
                }, GetOrBeginTransaction);

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
