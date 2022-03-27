using Dapper;
using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
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
            var finalSql = Queries.GetUserQuery.Replace("@@whereClause", Queries.GetWhereClause(request));

            return await DbConnection.QueryFirstOrDefaultAsync<Models.User>(finalSql, new { 
                userId = request.UserId, 
                emailAddress = request.EmailAddress, 
                userName = request.Username }, GetOrBeginTransaction);
        }
    }
}
