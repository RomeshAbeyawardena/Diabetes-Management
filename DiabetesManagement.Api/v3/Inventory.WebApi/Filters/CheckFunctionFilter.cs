using Inventory.Extensions;
using Inventory.Features.Function;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inventory.WebApi.Filters
{
    public class CheckFunctionFilter : IAsyncActionFilter
    {
        private readonly ILogger<CheckFunctionFilter> logger;
        private readonly IMediator mediator;
        private readonly ApplicationSettings applicationSettings;
        private const string ApiFunctionKey = "x-functions-key";
        public CheckFunctionFilter(ILogger<CheckFunctionFilter> logger, IMediator mediator, ApplicationSettings applicationSettings)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.applicationSettings = applicationSettings;

            if(applicationSettings == null)
            {
                throw new ArgumentNullException(nameof(applicationSettings));
            }
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var functionName = context.ActionDescriptor.DisplayName;
            var path = context.HttpContext.Request.Path;
            var function = await mediator.Send(new GetRequest { 
                Name = functionName,
                Path = path
            }, CancellationToken.None);

            if(function == null && applicationSettings.DiscoveryMode)
            {
                logger.LogWarning("Discovery mode enabled: Should be turned off in production mode once all endpoints have been populated");

                await mediator.Send(new PostCommand
                {
                    Name = functionName,
                    Path = path,
                    AccessToken = $"{functionName}|{path}|{Guid.NewGuid():N}"
                        .Hash(applicationSettings.HashAlgorithm!, applicationSettings.ConfidentialServerKey!)
                }, CancellationToken.None);

                await next();
            }
            var headers = context.HttpContext.Request.Headers;
            if (function!= null && headers.TryGetValue(ApiFunctionKey, out var functionsKey)
                && functionsKey == function.AccessToken)
            {
                await next();
            }

            context.Result = new UnauthorizedObjectResult(new Models.Response(StatusCodes.Status401Unauthorized, "Unauthorised function call"));
        }
    }
}
