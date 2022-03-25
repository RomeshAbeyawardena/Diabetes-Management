using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Api.RequestHandlers.ApiToken
{
    public class GetRequest : IRequest<Shared.Models.ApiToken>
    {
        public bool UseAuthenticatedContext { get; set; } = true;
        public string ApiKey { get; set; }
        public string ApiKeyChallenge { get; set; }
    }
}
