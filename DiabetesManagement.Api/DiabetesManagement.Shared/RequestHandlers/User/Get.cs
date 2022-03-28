using Dapper;
using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
using DiabetesManagement.Shared.Extensions;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.User
{
    [HandlerDescriptor(Queries.GetUser, Permissions.View)]
    public class Get : HandlerBase<GetRequest, Models.User>
    {
        public Get(string connectionString) : base(connectionString)
        {
        }

        public Get(IDbConnection dbConnection, IDbTransaction? dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        protected override void Dispose(bool disposing)
        {
            
        }

        protected override async Task<Models.User> HandleAsync(GetRequest request)
        {
            var user = new Models.User();

            var result = await user.Get(DbConnection, request, transaction: GetOrBeginTransaction);

            return result.FirstOrDefault()!;
        }
    }
}
