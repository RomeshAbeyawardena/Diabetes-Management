using DiabetesManagement.Shared.Contracts;
using System.Collections.Generic;

namespace DiabetesManagement.Api.RequestHandlers.ApiToken
{
    public class SaveRequest : IRequest<string>
    {
        public IEnumerable<byte> Key { get; set; }
        public string ApiKey { get; set; }
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
