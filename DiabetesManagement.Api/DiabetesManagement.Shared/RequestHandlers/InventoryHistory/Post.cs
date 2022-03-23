using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    public class Post : Handler
    {
        protected override void Dispose(bool disposing)
        {

        }

        private readonly Get getHandler;
        /// <summary>
        /// Initialises a new instance of the POST handler to initiate a new connection
        /// </summary>
        /// <param name="connectionString">The SQL connection string to be used to intialise the database connection</param>
        public Post(string connectionString)
            : base(connectionString)
        {
            getHandler = new(DbConnection);
            OnLoggerSet((logger) => getHandler.SetLogger = logger);
        }

        public Post(IDbConnection dbConnection, IDbTransaction dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {
            getHandler = new(dbConnection, dbTransaction);
            OnLoggerSet((logger) => getHandler.SetLogger = logger);
        }

        public async Task<Guid> Save(Models.InventoryHistory inventoryHistory)
        {
            TryOpenConnection();
            var dbTransaction = GetOrBeginTransaction;
            getHandler.UseTransaction = dbTransaction;
            var getRequest = new GetRequest
            {
                Key = inventoryHistory.Key,
                UserId = inventoryHistory.UserId,
                Type = inventoryHistory.Type
            };

            var inventory = await getHandler.GetInventory(getRequest);

            Logger.LogInformation("Finding existing INVENTORY...");

            var inventoryId = inventory?.InventoryId;
            var version = inventoryHistory.Version;

            if (version == default)
            {
                Logger.LogInformation("Finding INVENTORY_HISTORY to extract version information as a version has not been supplied");
                var inventoryHistoryRecord = await getHandler.GetInventoryHistory(getRequest);

                if (inventoryHistoryRecord != null)
                {
                    Logger.LogInformation("INVENTORY_HISTORY found with latest version at: {0}", inventoryHistoryRecord.Version);
                    version = inventoryHistoryRecord.Version + 1;
                }
                else
                    version = 1;
            }

            if (inventory == null)
            {
                Logger.LogInformation("INVENTORY not found, saving as a new entry....");
                inventoryId = await Save(inventoryHistory, getRequest, false, true);
            }
            else
            {
                Logger.LogInformation("INVENTORY found, updating entry....");
                inventory.Modified = DateTimeOffset.UtcNow;
                await UpdateInventory(inventory, getRequest, true);
            }

            Logger.LogInformation("Saving INVENTORY_HISTORY...");
            var result = await DbConnection.ExecuteScalarAsync<Guid>(Commands.InsertInventoryHistoryCommand,
                new
                {
                    inventoryHistoryId = inventoryHistory.InventoryHistoryId == default
                        ? Guid.NewGuid() : inventoryHistory.InventoryHistoryId,
                    inventoryId,
                    version,
                    type = inventoryHistory.Type ?? inventoryHistory.DefaultType,
                    items = inventoryHistory.Items,
                    created = inventoryHistory.Created == default
                        ? DateTimeOffset.UtcNow
                        : inventoryHistory.Created
                }, dbTransaction);

            Logger.LogInformation("Committing changes...");
            dbTransaction.Commit();

            return result;
        }
    }
}
