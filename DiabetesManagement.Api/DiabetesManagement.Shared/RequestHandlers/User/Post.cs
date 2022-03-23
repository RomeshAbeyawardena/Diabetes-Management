using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Shared.RequestHandlers.User
{
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
            return DbConnection.ExecuteScalarAsync<Guid>(Commands.InsertUser, new {
                userId = request.User!.UserId,
                emailAddress = request.User.EmailAddress,
                userName = request.User.Username,
                created = request.User.Created == default ? DateTime.UtcNow : request.User.Created,
            }, GetOrBeginTransaction);
        }
    }
}
