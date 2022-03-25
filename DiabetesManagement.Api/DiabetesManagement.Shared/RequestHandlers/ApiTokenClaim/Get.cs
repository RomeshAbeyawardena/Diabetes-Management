using Dapper;
using DiabetesManagement.Shared.Attributes;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.ApiTokenClaim
{
    [HandlerDescriptor(Queries.GetApiTokenClaims, Permissions.View)]
    public class Get : HandlerBase<GetRequest, IEnumerable<Models.ApiTokenClaim>>
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

        protected override Task<IEnumerable<Models.ApiTokenClaim>> HandleAsync(GetRequest request)
        {
            return DbConnection
                .QueryAsync<Models.ApiTokenClaim>(Queries.GetApiTokenClaimsQuery
                    .Replace("@@whereClause", Queries.GetApiTokenClaimsWhereClause(request)), 
                    GetOrBeginTransaction);
        }
    }
}
