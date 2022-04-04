namespace DiabetesManagement.Shared.RequestHandlers.ApiToken
{
    using Shared.Contracts;

    public class GetRequest : IRequest<Models.ApiToken>
    {
        public Guid? ApiTokenId { get; set; }
        public string? Key { get; set; }
        public string? Secret { get; set; }
    }
}
