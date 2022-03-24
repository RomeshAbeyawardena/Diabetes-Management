using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Shared.RequestHandlers.ApiTokenClaim
{
    public class GetRequest : IRequest<IEnumerable<Models.ApiTokenClaim>>
    {
        public Guid? ApiTokenId { get; set; }
    }
}
