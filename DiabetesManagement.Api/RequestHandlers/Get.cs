using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.RequestHandlers;
using System.Data;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers
{
    using Models = Shared.Models;
    using InventoryHistoryFeature = Shared.RequestHandlers.InventoryHistory;

    [HandlerDescriptor(Queries.GetInventoryItems)]
    public class Get : HandlerBase<GetRequest, Models.InventoryHistory>
    {
        public Get(string connectionString) : base(connectionString)
        {
        }

        public Get(IDbConnection dbConnection, IDbTransaction dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        protected override void Dispose(bool disposing)
        {

        }

        protected override Task<Models.InventoryHistory> HandleAsync(GetRequest request)
        {
            return HandlerFactory
                    .Execute<InventoryHistoryFeature.GetRequest, Models.InventoryHistory>(
                        InventoryHistoryFeature.Queries.GetInventoryHistory,
                        new InventoryHistoryFeature.GetRequest
                        {
                            Key = request.Key,
                            Type = request.Type,
                            UserId = request.UserId,
                            Version = request.Version
                        });
        }
    }
}
