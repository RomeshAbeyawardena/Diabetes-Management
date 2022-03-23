using System.Data;
using Dapper;
using DiabetesManagement.Shared.RequestHandlers.Inventory;
using Microsoft.Extensions.Logging;

namespace DiabetesManagement.Shared.RequestHandlers
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

        /// <summary>
        /// Initialises a new instance of the POST handler to reuse an existing connection and optional existing transaction objects
        /// </summary>
        /// <param name="dbConnection">An existing connection</param>
        /// <param name="dbTransaction">Optional existing transaction</param>
        public Post(IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {
            getHandler = new(dbConnection, dbTransaction);
            OnLoggerSet((logger) => getHandler.SetLogger = logger);
        }

        public async Task<Guid> Save(Models.Inventory inventory,
            Models.GetRequest? request = null,
            bool doValidationChecks = true,
            bool isInTransaction = false)
        {
            TryOpenConnection();
            var transaction = GetOrBeginTransaction;
            getHandler.UseTransaction = transaction;

            request ??= new Models.GetRequest
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
            var result = await DbConnection.ExecuteScalarAsync<Guid>(Commands.InsertInventoryCommand, new
            {
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
    }
}
