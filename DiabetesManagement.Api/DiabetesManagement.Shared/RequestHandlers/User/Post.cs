using Dapper;
using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
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

        protected override async Task<Guid> HandleAsync(SaveCommand request)
        {
            var transaction = GetOrBeginTransaction;
            if(request.User!.UserId == default)
            {
                request.User.UserId = Guid.NewGuid();
            }

            if (!string.IsNullOrEmpty(request.User.EmailAddress))
            {
                request.User.EmailAddress = request.User.EmailAddress.ToUpper().Encrypt("AES",
                    Convert.FromBase64String(ApplicationSettings.Instance!.ServerKey!),
                    Convert.FromBase64String(ApplicationSettings.Instance!.ServerInitialVector!));
            }

            if (!string.IsNullOrEmpty(request.User.Password))
            {
                request.User.Password = request.User.Password.Hash("SHA512", ApplicationSettings.Instance!.ServerKey!);
            }

            if(request.User!.Created == default)
            {
                request.User.Created = DateTimeOffset.UtcNow;
            }

            if (string.IsNullOrWhiteSpace(request.User.Hash))
            {
                request.User.Hash = request.User.GetHash();
            }

            var result = await request.User.Insert(DbConnection, transaction);

            if (request.CommitChanges)
            {
                transaction.Commit();
            }

            return result;
        }
    }
}
