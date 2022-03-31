using AutoMapper;
using DiabetesManagement.Shared;
using DiabetesManagement.Shared.Contracts;
using DiabetesManagement.Shared.RequestHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DiabetesManagement.Api
{
    using ApiKeyFeature = RequestHandlers.ApiToken;
    using DbModels = Shared.Models;

    public abstract class ApiBase : IDisposable
    {
        protected async Task<bool> AuthenticateRequest(HttpRequest httpRequest)
        {
            httpRequest.Headers.TryGetValue("X-API-KEY", out var apiKey);
            httpRequest.Headers.TryGetValue("X-API-CHALLENGE", out var apiChallenge);

            var apiToken = await HandlerFactory
                .Execute<ApiKeyFeature.GetRequest, DbModels.ApiToken>(
                    ApiKeyFeature.Queries.GetValidatedApiToken,
                    new ApiKeyFeature.GetRequest
                    {
                        Key = Convert.FromBase64String(ApplicationSettings.SigningKey),
                        ValidAudience = ApplicationSettings.Audience,
                        ValidIssuer = ApplicationSettings.Issuer,
                        ApiKey = apiKey,
                        ApiKeyChallenge = apiChallenge,
                        UseAuthenticatedContext = false,
                    });

            if (await HandlerFactory.IsAuthenticated(apiToken))
            {
                return true;
            }

            throw new UnauthorizedAccessException();
        }

        protected IActionResult HandleException(Exception exception)
        {
            Logger.LogError(exception, "A handled error has occurred");
            return new BadRequestObjectResult(exception.Message);
        }


        protected ILogger Logger { get; }
        protected ApplicationSettings ApplicationSettings { get; }
        protected IAuthenticatedHandlerFactory HandlerFactory { get; }
        protected IMapper Mapper { get; }

        public ApiBase(ILogger log, IConfiguration configuration)
        {
            var assemblies = Shared.RequestHandlers.HandlerFactory.GetAssemblies(typeof(ApiBase).Assembly);

            var connectionString = configuration.GetConnectionString("Default");
            Mapper = new MapperConfiguration(cfg => cfg.AddMaps(assemblies)).CreateMapper();
            HandlerFactory = new AuthenticatedHandlerFactory(connectionString, log, Mapper, assemblies);
            
        }

        public virtual void Dispose()
        {
            HandlerFactory?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
