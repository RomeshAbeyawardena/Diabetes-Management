using Dapper;
using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Extensions;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.User
{
    [HandlerDescriptor(Commands.SaveUser, Permissions.Add, Permissions.Edit)]
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
            if(request.User!.Created == default)
            {
                request.User.Created = DateTimeOffset.UtcNow;
            }

            return DbConnection.ExecuteScalarAsync<Guid>(Commands.InsertUser, new {
                userId = request.User!.UserId,
                emailAddress = request.User.EmailAddress,
                userName = request.User.Username,
                hash = request.User.Hash ?? request.User.GetHash(),
                created = request.User.Created,
            }, GetOrBeginTransaction);
        }
    }
}
