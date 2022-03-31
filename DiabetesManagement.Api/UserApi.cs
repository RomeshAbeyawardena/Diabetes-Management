using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DiabetesManagement.Api
{
    public class UserApi : ApiBase
    {
        public UserApi(ILogger<UserApi> logger, IConfiguration configuration)
            : base(logger, configuration)
        {

        }

        [FunctionName("Register")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] 
            HttpRequest request)
        {
            var requiredConditions = new[]
            {
                request.Query.TryGetValue("emailAddress", out var emailAddress),
                request.Query.TryGetValue("displayName", out var displayName),
                request.Query.TryGetValue("password", out var password)
            };

            if(requiredConditions.All(a => a))
            {
                
            }

            return new OkResult();
        }
    }
}
