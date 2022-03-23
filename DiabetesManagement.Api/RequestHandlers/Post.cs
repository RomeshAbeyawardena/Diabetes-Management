using Dapper;
using DiabetesManagement.Api.Models;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DiabetesManagement.Api.RequestHandlers
{
    /// <summary>
    /// Represents a POST handler
    /// </summary>
    public class Post : Handler
    {
        private readonly Get getHandler;

        /// <summary>
        /// Initialises a new instance of the POST handler to initiate a new connection
        /// </summary>
        /// <param name="connectionString">The SQL connection string to be used to intialise the database connection</param>
        public Post(string connectionString)
            : base(connectionString)
        {
            getHandler = new(DbConnection);
        }

        /// <summary>
        /// Initialises a new instance of the POST handler to reuse an existing connection and optional existing transaction objects
        /// </summary>
        /// <param name="dbConnection">An existing connection</param>
        /// <param name="dbTransaction">Optional existing transaction</param>
        public Post(IDbConnection dbConnection, IDbTransaction dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {
            getHandler = new(dbConnection, dbTransaction);
        }

        protected override void Dispose(bool disposing)
        {
            
        }

        /// <summary>
        /// Gets an instance of the GET handler
        /// </summary>
        public Get Get => getHandler;

        public async Task<Guid> UpdateInventory(Inventory inventory, GetRequest request, bool isInTransaction = false)
        {
            TryOpenConnection();
            var transaction = GetOrBeginTransaction;

            request ??= new GetRequest
            {
                Key = inventory.Key,
                UserId = inventory.UserId,
            };

            request.InventoryId = inventory.InventoryId == default ? null : inventory.InventoryId;

            var inventoryRecord = await getHandler.GetInventory(request);

            if (inventoryRecord == null)
            {
                throw new DataException("Inventory record not found");
            }

            
            var result = await DbConnection.ExecuteScalarAsync<Guid>(Commands.UpdateInventoryCommand, new { 
                key = inventory.Key, 
                defaultType = inventory.DefaultType,
                modified = inventory.Modified, 
                inventoryRecord.InventoryId }, transaction);

            if (!isInTransaction)
            {
                transaction.Commit();
            }

            return result;
        }

        /// <summary>
        /// Saves the inventory to a data store
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="doValidationChecks"></param>
        /// <param name="isInTransaction"></param>
        /// <returns></returns>
        /// <exception cref="DataException"></exception>
        public async Task<Guid> Save(Inventory inventory, 
            GetRequest request = null,
            bool doValidationChecks = true, 
            bool isInTransaction = false)
        {
            TryOpenConnection();
            var transaction = GetOrBeginTransaction;
            getHandler.UseTransaction = transaction;

            request ??= new GetRequest
            {
                Key = inventory.Key,
                UserId = inventory.UserId,
                Type = inventory.DefaultType
            };

            if (doValidationChecks)
            {
                Logger.LogInformation("Checking INVENTORY exists...");
                var inventoryRecord = await getHandler.GetInventory(request);

                if (inventoryRecord != null)
                {
                    throw new DataException("Inventory record already exists");
                }
            }
            else
            {
                Logger.LogInformation("Skipping validation checks");
            }

            Logger.LogInformation("Saving INVENTORY...");
            var result = await DbConnection.ExecuteScalarAsync<Guid>(Commands.InsertInventoryCommand, new {
                inventoryId = inventory.InventoryId == default ? Guid.NewGuid() : inventory.InventoryId,
                key = inventory.Key,
                userId = inventory.UserId,
                defaultType = inventory.DefaultType,
                created = inventory.Created == default ? DateTimeOffset.UtcNow : inventory.Created
            }, transaction);

            if (!isInTransaction)
            {
                Logger.LogInformation("Committing changes...");
                transaction.Commit();
            }

            return result;
        }

        /// <summary>
        /// Saves the inventory history to a data store
        /// </summary>
        /// <param name="inventoryHistory"></param>
        /// <returns></returns>
        public async Task<Guid> Save(InventoryHistory inventoryHistory)
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

                if(inventoryHistoryRecord != null)
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
                new { 
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
