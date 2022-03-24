using Dapper;
using DiabetesManagement.Shared.Attributes;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.ApiToken
{
    [HandlerDescriptor(Commands.SaveApiToken, Permissions.Add, Permissions.Edit)]
    public class Post : HandlerBase<SaveCommand, Guid>
    {
        public Post(string connectionString) : base(connectionString)
        {
        }

        public Post(IDbConnection dbConnection, IDbTransaction? dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        protected override void Dispose(bool disposing)
        {
            
        }

        protected override Task<Guid> HandleAsync(SaveCommand request)
        {
            return DbConnection.ExecuteScalarAsync<Guid>(Commands.InsertApiTokenCommand, new
            {
                @apiTokenId = request.ApiToken!.ApiTokenId == default ? Guid.NewGuid() : request.ApiToken.ApiTokenId,
                @key = request.ApiToken.Key,
                @secret = request.ApiToken.Secret,
                @created = request.ApiToken.Created == default ? DateTimeOffset.UtcNow : request.ApiToken.Created
            }, GetOrBeginTransaction);
        }
    }
}
