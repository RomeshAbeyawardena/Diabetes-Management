using Inventory.Extensions;
using Inventory.Features.Function;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inventory.WebApi.Filters
{
    public class CheckFunctionFilter : IAsyncActionFilter, IDisposable
    {
        void IDisposable.Dispose()
        {
            cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }

        private readonly ILogger<CheckFunctionFilter> logger;
        private readonly IMediator mediator;
        private readonly ApplicationSettings applicationSettings;
        private readonly CancellationTokenSource cancellationTokenSource;
        private const string ApiFunctionKey = "x-functions-key";
        public CheckFunctionFilter(ILogger<CheckFunctionFilter> logger, IMediator mediator, ApplicationSettings applicationSettings)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.applicationSettings = applicationSettings;
            cancellationTokenSource = new();

            if (applicationSettings == null)
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
            }, cancellationTokenSource.Token);

            if(function == null && applicationSettings.DiscoveryMode)
            {
                logger.LogWarning("Discovery mode enabled: Should be turned off in production mode once all endpoints have been populated");

                await mediator.Send(new PostCommand
                {
                    Name = functionName,
                    Path = path,
                    AccessToken = $"{functionName}|{path}|{Guid.NewGuid():N}"
                        .Hash(applicationSettings.HashAlgorithm!, applicationSettings.ConfidentialServerKey!)
                }, cancellationTokenSource.Token);

                await next();
                return;
            }
            var headers = context.HttpContext.Request.Headers;
            if (function!= null && headers.TryGetValue(ApiFunctionKey, out var functionsKey)
                && functionsKey == function.AccessToken)
            {
                await next();
                return;
            }

            context.Result = new UnauthorizedObjectResult(new Models.Response(StatusCodes.Status401Unauthorized, "Unauthorised function call"));
        }
    }
}
