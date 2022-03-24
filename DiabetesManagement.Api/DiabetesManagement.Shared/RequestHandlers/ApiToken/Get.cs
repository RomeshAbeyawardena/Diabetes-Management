using Dapper;
using DiabetesManagement.Shared.Attributes;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.ApiToken
{
    [HandlerDescriptor(Queries.GetApiToken, Permissions.View)]
    public class Get : HandlerBase<GetRequest, Models.ApiToken>
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

        protected override async Task<Models.ApiToken> HandleAsync(GetRequest request)
        {
            return await DbConnection.QueryFirstOrDefault(Queries.GetApiTokenQuery.Replace("@@whereClause", Queries.GetApiTokenWhereClause(request)), 
                new {
                    key = request.Key,
                    secret = request.Secret
                }, GetOrBeginTransaction);
        }
    }
}
