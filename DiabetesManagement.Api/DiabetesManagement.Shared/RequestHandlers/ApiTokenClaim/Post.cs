using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
using System.Data;

namespace DiabetesManagement.Shared.RequestHandlers.ApiTokenClaim
{
    [HandlerDescriptor(Commands.SaveApiTokenClaim, Permissions.Add, Permissions.Edit)]
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
            throw new NotImplementedException();
        }
    }
}
