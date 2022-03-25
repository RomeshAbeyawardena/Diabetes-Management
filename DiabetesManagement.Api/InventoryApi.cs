using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DiabetesManagement.Api
{
    using DiabetesManagement.Api.RequestHandlers.InventoryHistory;
    using DiabetesManagement.Shared.Contracts;
    using DiabetesManagement.Shared.RequestHandlers;
    using DbModels = Shared.Models;
    using ApiKeyFeature = RequestHandlers.ApiToken;
    using DiabetesManagement.Shared;

    public class InventoryApi : IDisposable
    {
        private readonly IAuthenticatedHandlerFactory handlerFactory;
        private readonly ApplicationSettings applicationSettings;
        private readonly ILogger<InventoryApi> _logger;
        
        private async Task<bool> AuthenticateRequest(HttpRequest httpRequest)
        {
            httpRequest.Headers.TryGetValue("X-API-KEY", out var apiKey);
            httpRequest.Headers.TryGetValue("X-API-CHALLENGE", out var apiChallenge);

            var apiToken = await handlerFactory
                .Execute<ApiKeyFeature.GetRequest, DbModels.ApiToken>(
                    ApiKeyFeature.Queries.GetValidatedApiToken, 
                    new ApiKeyFeature.GetRequest { 
                        Key = Convert.FromBase64String(applicationSettings.SigningKey),
                        ApiKey = apiKey, 
                        ApiKeyChallenge = apiChallenge,
                        UseAuthenticatedContext = false,
                    });

            if (await handlerFactory.IsAuthenticated(apiToken))
            {
                return true;
            }

            throw new UnauthorizedAccessException();
        }

        private IActionResult HandleException(Exception exception)
        {
            _logger.LogError(exception, "A handled error has occurred");
            return new BadRequestObjectResult(exception.Message);
        }

        public InventoryApi(ILogger<InventoryApi> log, IConfiguration configuration)
        {
            _logger = log;
            this.applicationSettings = new ApplicationSettings(configuration);
            var connectionString = configuration.GetConnectionString("Default");
            handlerFactory = new AuthenticatedHandlerFactory(connectionString, log, HandlerFactory.GetAssemblies(typeof(InventoryApi).Assembly));
        }

        [FunctionName("GetInventory")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "get" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "key", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **key** parameter")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Description = "The **userId** parameter")]
        [OpenApiParameter(name: "version", In = ParameterLocation.Query, Required = false, Type = typeof(int?), Description = "The **version** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DbModels.InventoryHistory), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest request)
        {
            await AuthenticateRequest(request);

            var requiredConditions = new[]
            {
                request.Query.TryGetValue("key", out var key),
                request.Query.TryGetValue("type", out var type),
                request.Query.TryGetValue("userId", out var userIdValue)
            };

            if (requiredConditions.All(a => a) && Guid.TryParse(userIdValue, out var userId))
            {
                int? versionNumber = null;

                if (request.Query.TryGetValue("version", out var version) 
                    && int.TryParse(version, out int versionNum))
                {
                    versionNumber = versionNum;
                }

                var inventory = await handlerFactory
                    .Execute<GetRequest, DbModels.InventoryHistory>(
                        Queries.GetInventoryItems, 
                        new GetRequest
                        {
                            Key = key,
                            Type = type,
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
        public async Task<IActionResult> SaveInventory([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request)
        {
            try
            {
                await AuthenticateRequest(request);

                DbModels.InventoryHistory savedEntity = null;

                var requiredConditions = new[] { request.Form.TryGetValue("items", out var items),
                request.Form.TryGetValue("type", out var type),
                request.Form.TryGetValue("key", out var key),
                request.Form.TryGetValue("userId", out var userIdValue) };

                if (requiredConditions.All(a => a))
                {
                    if (Guid.TryParse(userIdValue, out var userId))
                    {
                        savedEntity = await handlerFactory
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

                return new OkObjectResult(savedEntity);
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

        public void Dispose()
        {
            handlerFactory?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

