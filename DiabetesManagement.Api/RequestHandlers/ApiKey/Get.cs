using DiabetesManagement.Shared.RequestHandlers;
using System.Data;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers.ApiKey
{
    public class Get : HandlerBase<GetRequest>
    {
        public Get(string connectionString) : base(connectionString)
        {
        }

        public Get(IDbConnection dbConnection, IDbTransaction dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        public override Task Handle(GetRequest request)
        {
            throw new System.NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            
        }
    }
}
