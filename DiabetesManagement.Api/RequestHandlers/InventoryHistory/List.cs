namespace DiabetesManagement.Api.RequestHandlers.InventoryHistory
{
    using Models = Shared.Models;
    using InventoryHistoryFeature = Shared.RequestHandlers.InventoryHistory;
    using DiabetesManagement.Shared.Base;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using DiabetesManagement.Shared.Attributes;

    [HandlerDescriptor(Queries.ListInventoryHistory)]
    public class List : HandlerBase<ListRequest, IEnumerable<Models.InventoryHistory>>
    {
        public List(string connectionString) : base(connectionString)
        {
        }

        public List(IDbConnection dbConnection, IDbTransaction dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        protected override void Dispose(bool disposing)
        {

        }

        protected override Task<IEnumerable<Models.InventoryHistory>> HandleAsync(ListRequest request)
        {
            return HandlerFactory
                .Execute<InventoryHistoryFeature.ListRequest, IEnumerable<Models.InventoryHistory>>(
                    InventoryHistoryFeature.Queries.ListInventoryHistory, new InventoryHistoryFeature.ListRequest
                    {
                        Key = request.Key,
                        Type = request.Type,
                        UserId = request.UserId
                    });
        }
    }
}
