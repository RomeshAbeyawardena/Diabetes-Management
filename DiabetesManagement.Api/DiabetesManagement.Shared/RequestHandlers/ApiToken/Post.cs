using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.ApiToken
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
            throw new NotImplementedException();
        }

        protected override Task<Guid> HandleAsync(SaveCommand request)
        {
            throw new NotImplementedException();
        }
    }
}
