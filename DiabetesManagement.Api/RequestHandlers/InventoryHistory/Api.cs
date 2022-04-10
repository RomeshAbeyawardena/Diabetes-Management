using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers.InventoryHistory
{
    using DbModels = Shared.Models;
    using DiabetesManagement.Shared.Extensions;

    public class Api : ApiBase
    {
        public const string BaseUrl = "Inventory";
        public Api(ILogger<Api> log, IConfiguration configuration)
            : base(log, configuration)
        {
            
        }

        [OpenApiOperation(operationId: "GetInventory", 
            tags: new[] { "get" }, 
            Summary = "Retrieves an inventory for a specified user", 
            Description = "Retrieves an inventory for a specified user by key and type", 
            Visibility = OpenApiVisibilityType.Important),
        OpenApiParameter(name: "key", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The key", Description = "The key", Visibility = OpenApiVisibilityType.Important),
        OpenApiParameter(name: "type", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The type", Description = "The type", Visibility = OpenApiVisibilityType.Important),
        OpenApiParameter(name: "userId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The userId", Description = "The userId", Visibility = OpenApiVisibilityType.Important),
        //OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(DbModels.InventoryHistory)),
        FunctionName("GetInventory")]
        public async Task<IActionResult> GetInventory(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = BaseUrl)] HttpRequest request)
        {
            //await AuthenticateRequest(request);
            Guid inventoryHistoryId = default;
            var requiredConditionsA = new[]
            {
                request.Query.TryGetValue("inventoryHistoryId", out var id) && Guid.TryParse(id.ToString(), out inventoryHistoryId),
            };

            var requiredConditionsB = new[]
            {
                request.Query.TryGetValue("key", out var key),
                request.Query.TryGetValue("type", out var type),
                request.Query.TryGetValue("userId", out var userIdValue)
            };

            if ((requiredConditionsA.All(a => a) || requiredConditionsB.All(a => a)) && Guid.TryParse(userIdValue, out var userId))
            {
                int? versionNumber = null;

                if (request.Query.TryGetValue("version", out var version) 
                    && int.TryParse(version, out int versionNum))
                {
                    versionNumber = versionNum;
                }

                var inventory = await HandlerFactory
                    .Execute<GetRequest, DbModels.InventoryHistory>(
                        Queries.GetInventoryItems, 
                        new GetRequest
                        {
                            Key = key,
                            Type = type,
                            UserId = userId,
                            Version = versionNumber
                        });

                return new OkObjectResult(inventory.ToDynamic());
            }

            return new BadRequestResult();
        }

        [OpenApiOperation(operationId: "SaveInventory",
            tags: new[] { "post" },
            Summary = "Save an inventory for a specified user",
            Description = "Saves an inventory for a specified user by key and type",
            Visibility = OpenApiVisibilityType.Important),
         OpenApiRequestBody("multipart/form-data", typeof(SaveRequest), Description = "Inventory to save", Required = true),
         FunctionName("SaveInventory")]
        public async Task<IActionResult> SaveInventory([HttpTrigger(AuthorizationLevel.Function, "post", Route = BaseUrl)] HttpRequest request)
        {
            try
            {
                //await AuthenticateRequest(request);

                DbModels.InventoryHistory savedEntity = null;

                var requiredConditions = new[] { request.Form.TryGetValue("items", out var items),
                request.Form.TryGetValue("type", out var type),
                request.Form.TryGetValue("key", out var key),
                request.Form.TryGetValue("userId", out var userIdValue) };

                if (requiredConditions.All(a => a))
                {
                    if (Guid.TryParse(userIdValue, out var userId))
                    {
                        savedEntity = await HandlerFactory
                            .Execute<SaveRequest, DbModels.InventoryHistory>(
                                Commands.SaveInventoryPayload, 
                                new SaveRequest
                                {
                                    Items = items,
                                    Type = type,
                                    Key = key,
                                    UserId = userId
                                });
                    }
                    else
                        throw new InvalidOperationException("User id is in an invalid format");
                }
                else
                {
                    throw new InvalidOperationException($"Expected form values for items, type, key, userId. " +
                        $"Received: { (requiredConditions[0] ? "items" : "") }," +
                        $" { (requiredConditions[1] ? "type" : "") }, " +
                        $" { (requiredConditions[2] ? "key" : "") }, " +
                        $" { (requiredConditions[3] ? "userId" : "") }");
                }

                return new OkObjectResult(savedEntity.ToDynamic());
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return HandleException(invalidOperationException);
            }
            catch (DataException dataException)
            {
                return HandleException(dataException);
            }
        }
    }
}

