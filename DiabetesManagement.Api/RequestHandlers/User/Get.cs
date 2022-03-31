﻿using System.Data;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers.User
{
    using UserFeature = Shared.RequestHandlers.User;
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
            return HandlerFactory.Execute<UserFeature.GetRequest, Shared.Models.User>(
                UserFeature.Queries.GetUser, 
                new UserFeature.GetRequest { 
                    EmailAddress = request.EmailAddress
                });
        }
    }
}
