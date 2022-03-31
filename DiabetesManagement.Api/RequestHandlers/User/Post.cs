using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers.User
{
    using UserFeature = Shared.RequestHandlers.User;
    [HandlerDescriptor(Commands.RegisterUser)]
    public class Post : HandlerBase<SaveRequest, Shared.Models.User>
    {
        public Post(string connectionString) : base(connectionString)
        {
        }

        public Post(IDbConnection dbConnection, IDbTransaction dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        protected override void Dispose(bool disposing)
        {
            
        }

        protected override async Task<Shared.Models.User> HandleAsync(SaveRequest request)
        {
            //check if already exists
            var user = await HandlerFactory.Execute<GetRequest, Shared.Models.User>(Queries.GetUser, new GetRequest { EmailAddress = request.EmailAddress });

            if(user != null)
            {
                throw new InvalidOperationException("User already exists");
            }

            var userId = await HandlerFactory.Execute<UserFeature.SaveCommand, Guid>(
                UserFeature.Commands.SaveUser, 
                new UserFeature.SaveCommand { User = new Shared.Models.User {
                    DisplayName = request.DisplayName,
                    EmailAddress = request.EmailAddress,
                    Password = request.Password
                }});

            return await HandlerFactory.Execute<UserFeature.GetRequest, Shared.Models.User>(
                UserFeature.Queries.GetUser, new UserFeature.GetRequest { UserId = userId });
        }
    }
}
