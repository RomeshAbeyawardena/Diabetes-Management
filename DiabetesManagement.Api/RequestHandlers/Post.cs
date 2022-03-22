using Dapper;
using DiabetesManagement.Api.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers
{
    public class Post : Handler
    {
        private readonly Get getHandler;

        public Post(string connectionString)
            : base(connectionString)
        {
            getHandler = new(DbConnection);
        }

        public Post(IDbConnection dbConnection, IDbTransaction dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {
            getHandler = new(dbConnection, dbTransaction);
        }

        protected override void Dispose(bool disposing)
        {
            
        }

        public Get Get => getHandler;

        public async Task<Guid> Save(Inventory inventory, bool doValidationChecks = true, bool isInTransaction = false)
        {
            var transaction = GetOrBeginTransaction;
            getHandler.UseTransaction = transaction;
            var getRequest = new GetRequest
            {
                Key = inventory.Key,
                UserId = inventory.UserId,
            };

            if (doValidationChecks)
            {
                var inventoryRecord = await getHandler.GetInventory(getRequest);

                if (inventoryRecord != null)
                {
                    throw new DataException("Inventory record already exists");
                }
            }

            var command = @"INSERT INTO [dbo].[Inventory] (
                [INVENTORYID],
	            [KEY],
	            [USERID],
	            [CREATED]
            ) VALUES (
                @inventoryId,
                @key,
                @userId,
                @created
            ); SELECT @inventoryId";

            TryOpenConnection();

            var result = await DbConnection.ExecuteScalarAsync<Guid>(command, new {
                inventoryId = inventory.InventoryId == default ? Guid.NewGuid() : inventory.InventoryId,
                key = inventory.Key,
                userId = inventory.UserId,
                created = inventory.Created == default ? DateTimeOffset.UtcNow : inventory.Created
            }, transaction);

            if (!isInTransaction)
            {
                transaction.Commit();
            }

            return result;
        }

        public async Task<Guid> Save(InventoryHistory inventoryHistory)
        {
            TryOpenConnection();
            var dbTransaction = GetOrBeginTransaction;
            getHandler.UseTransaction = dbTransaction;
            var getRequest = new GetRequest
            {
                Key = inventoryHistory.Key,
                UserId = inventoryHistory.UserId,
            };

            var inventory = await getHandler.GetInventory(getRequest);

            var inventoryId = inventory?.InventoryId;
            var version = inventoryHistory.Version;

            if (version == default)
            {
                var inventoryHistoryRecord = await getHandler.GetInventoryHistory(getRequest);

                version = inventoryHistoryRecord != null ? inventoryHistoryRecord.Version + 1 : 1;
            }

            if (inventory == null)
            {
                inventoryId = await Save(inventoryHistory, false, true);
            }

            var command = @"INSERT INTO [dbo].[INVENTORY_HISTORY] (
                                [INVENTORY_HISTORYID],
                                [INVENTORYID],
                                [VERSION],
                                [ITEMS],
                                [CREATED]
                            ) VALUES (
                                @inventoryHistoryId,
                                @inventoryId,
                                @version,
                                @items,
                                @created
                            ); SELECT @inventoryHistoryId";

            TryOpenConnection();

            var result = await DbConnection.ExecuteScalarAsync<Guid>(command, 
                new { 
                    inventoryHistoryId = inventoryHistory.InventoryHistoryId == default
                        ? Guid.NewGuid() : inventoryHistory.InventoryHistoryId,
                    inventoryId,
                    version,
                    items = inventoryHistory.Items,
                    created = inventoryHistory.Created == default 
                        ? DateTimeOffset.UtcNow
                        : inventoryHistory.Created
                }, dbTransaction);

            dbTransaction.Commit();

            return result;
        }
    }
}
