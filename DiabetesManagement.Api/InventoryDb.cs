using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DiabetesManagement.Api.Models;
using DiabetesManagement.Api.RequestHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace DiabetesManagement.Api
{
    public class InventoryDb
    {
        private readonly ILogger<InventoryDb> _logger;
        private const string ConnectionString = "Server=dotnetinsights.database.windows.net;Initial Catalog=DiabetesUnitsManager;User Id=romesh.a;password=e138llRA1787!;MultipleActiveResultSets=true";
        public InventoryDb(ILogger<InventoryDb> log)
        {
            _logger = log;
        }

        [FunctionName("GetInventory")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "get" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "key", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **key** parameter")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Description = "The **userId** parameter")]
        [OpenApiParameter(name: "version", In = ParameterLocation.Query, Required = false, Type = typeof(Guid), Description = "The **version** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(InventoryHistory), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest request)
        {
            int? versionNumber = null;

            var requiredConditions = new[]
            {
                request.Query.TryGetValue("key", out var key),
                request.Query.TryGetValue("userId", out var userIdValue)
            };

            if(requiredConditions.All(a => a) && Guid.TryParse(userIdValue, out var userId))
            {
                if (request.Query.TryGetValue("version", out var version) && int.TryParse(version, out int versionNum))
                {
                    versionNumber = versionNum;
                }

                using var getHandler = new Get(ConnectionString);

                var inventory = await getHandler.GetInventory(new GetRequest
                {
                    Key = key,
                    UserId = userId,
                    Version = versionNumber
                });

                return new OkObjectResult(inventory);
            }

            return new BadRequestResult();
        }

        [FunctionName("SaveInventory")]
        [OpenApiOperation(operationId: "Save", tags: new[] { "post" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        public async Task<IActionResult> Save([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request)
        {
            using var postHandler = new Post(ConnectionString);

            var requiredConditions = new[] { request.Form.TryGetValue("items", out var items),
                request.Form.TryGetValue("key", out var key),
                request.Form.TryGetValue("userId", out var userIdValue) };
           
            if(requiredConditions.All(a => a) && Guid.TryParse(userIdValue, out var userId))
            {
                await postHandler.Save(new InventoryHistory { Items = items, Key = key, UserId = userId });
            }
            else
            {
                return new BadRequestResult();
            }

            return new OkResult();
        }
    }
}

