using DiabetesManagement.Shared.Attributes;
using System.Data;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers.User
{
    using UserFeature = Shared.RequestHandlers.User;
    [HandlerDescriptor(Queries.GetUser)]
    public class Get : Shared.Base.HandlerBase<GetRequest, Shared.Models.User>
    {
        public Get(string connectionString) : base(connectionString)
        {
        }

        public Get(IDbConnection dbConnection, IDbTransaction dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        protected override void Dispose(bool disposing)
        {

        }

        protected override Task<Shared.Models.User> HandleAsync(GetRequest request)
        {
            if (request.AuthenticateUser)
            {
                return HandlerFactory.Execute<UserFeature.GetRequest, Shared.Models.User>(
                    UserFeature.Queries.GetUser,
                    new UserFeature.GetRequest
                    {
                        EmailAddress = request.EmailAddress,
                        Password = request.Password
                    });
            }

            return HandlerFactory.Execute<UserFeature.GetRequest, Shared.Models.User>(
                    UserFeature.Queries.GetUser,
                    new UserFeature.GetRequest
                    {
                        EmailAddress = request.EmailAddress
                    });
        }
    }
}
