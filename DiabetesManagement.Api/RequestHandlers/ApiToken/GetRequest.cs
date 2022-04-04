using DiabetesManagement.Shared.Contracts;
using System.Collections.Generic;

namespace DiabetesManagement.Api.RequestHandlers.ApiToken
{
    public class GetRequest : IRequest<Shared.Models.ApiToken>
    {
        public IEnumerable<byte> Key { get; set; }
        public bool UseAuthenticatedContext { get; set; } = true;
        public string ApiKey { get; set; }
        public string ApiKeyChallenge { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
    }
}
