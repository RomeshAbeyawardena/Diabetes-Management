using DiabetesManagement.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers.User
{
    public class Api : ApiBase
    {
        public const string BaseUrl = "User";
        public Api(ILogger<Api> logger, IConfiguration configuration)
            : base(logger, configuration)
        {

        }

        [FunctionName("Login")]
        public async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Function, "post", Route = BaseUrl)]
            HttpRequest request)
        {
            var requiredConditions = new[]
            {
                request.Query.TryGetValue("emailAddress", out var emailAddress),
                request.Query.TryGetValue("password", out var password)
            };

            if (requiredConditions.All(a => a))
            {
                var requestedUser = await HandlerFactory.Execute<GetRequest, Shared.Models.User>(Queries.GetUser, new GetRequest
                {
                    EmailAddress = emailAddress,
                    AuthenticateUser = true,
                    Password = password
                });

                if (requestedUser == null)
                {
                    return new UnauthorizedResult();
                }

                return new OkObjectResult(requestedUser.ToDynamic());
            }

            return new BadRequestResult();
        }


        [FunctionName("Register")]
        public async Task<IActionResult> Register([HttpTrigger(AuthorizationLevel.Function, "post", Route = $"{BaseUrl}/{nameof(Register)}")] 
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
                var savedUser = await HandlerFactory.Execute<SaveRequest, Shared.Models.User>(
                    Queries.GetUser, 
                    new SaveRequest { 
                        DisplayName = displayName, 
                        EmailAddress = emailAddress, 
                        Password = password 
                    });

                return new OkObjectResult(savedUser.ToDynamic());
            }

            return new BadRequestResult();
        }
    }
}
