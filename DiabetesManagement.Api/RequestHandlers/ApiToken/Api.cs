using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers.ApiToken
{
    public class Api : ApiBase
    {
        public const string BaseUrl = "Token";
        public Api(ILogger log, IConfiguration configuration) : base(log, configuration)
        {
        }

        [FunctionName("Challenge")]
        public async Task<IActionResult> Challenge([HttpTrigger(AuthorizationLevel.Function, "post", Route = BaseUrl)] HttpRequest request)
        {
            var requiredFields = new[]
            {
                request.Form.TryGetValue("apiKey", out var apiKey),
                request.Form.TryGetValue("apiSecret", out var secret)
            };

            if (requiredFields.All(a => a))
            {
                var result = await HandlerFactory.Execute<SaveRequest, string>(Commands.SaveApiTokenChallenge, new SaveRequest
                {
                    ApiKey = apiKey,
                    Secret = secret,
                    Key = Convert.FromBase64String(ApplicationSettings.SigningKey),
                    Audience = ApplicationSettings.Audience,
                    Issuer = ApplicationSettings.Issuer,

                });

                return new OkObjectResult(result);
            }

            return new BadRequestResult();
        }
    }
}
