using Dapper;
using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
using DiabetesManagement.Shared.Extensions;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    [HandlerDescriptor(Commands.SaveInventoryHistory, Permissions.Add, Permissions.Edit)]
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

        public Post(IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
            : base(dbConnection, dbTransaction)
        {
            
        }

        protected override async Task<Guid> HandleAsync(SaveCommand request)
        {
            var dbTransaction = GetOrBeginTransaction;
            var inventoryHistory = request.InventoryHistory;

            var getRequest = new GetRequest
            {
                InventoryId = inventoryHistory!.InventoryId,
                Type = inventoryHistory.Type,
                IsLatest = true,
            };

            var version = inventoryHistory.Version;

            if (version == default)
            {
                var inventoryHistoryRecord = await HandlerFactory!.Execute<GetRequest, Models.InventoryHistory>(Queries.GetInventoryHistory, getRequest);

                if (inventoryHistoryRecord != null)
                {
                    Logger.LogInformation("INVENTORY_HISTORY found with latest version at: {version}", inventoryHistoryRecord.Version);
                    inventoryHistory.Version = inventoryHistoryRecord.Version + 1;
                }
                else
                    inventoryHistory.Version = 1;
            }

            if (string.IsNullOrWhiteSpace(inventoryHistory.Type))
            {
                inventoryHistory.Type = inventoryHistory.Type;
            }

            if (inventoryHistory.InventoryHistoryId == default)
            {
                inventoryHistory.InventoryHistoryId = Guid.NewGuid();
            }

            if (inventoryHistory.Created == default)
            {
                inventoryHistory.Created = DateTimeOffset.UtcNow;
            }

            if(string.IsNullOrWhiteSpace(inventoryHistory.Hash))
            {
                inventoryHistory.Hash = inventoryHistory.GetHash();
            }

            Logger.LogInformation("Saving INVENTORY_HISTORY...");

            var result = await inventoryHistory.Insert(DbConnection, GetOrBeginTransaction);

            if (request.CommitOnCompletion)
            {
                Logger.LogInformation("Committing changes...");
                dbTransaction.Commit();
            }

            return result;

        }
    }
}
