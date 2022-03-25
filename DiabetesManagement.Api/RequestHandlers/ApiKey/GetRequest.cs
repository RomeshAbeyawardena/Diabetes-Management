using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Api.RequestHandlers.ApiKey
{
    public class GetRequest : IRequest<Shared.Models.ApiToken>
    {
        public string ApiKey { get; set; }
        public string ApiKeyChallenge { get; set; }
    }
}
