using Inventory.Features.Function;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inventory.WebApi.Filters
{
    public class CheckFunctionFilter : IAsyncActionFilter
    {
        private readonly IMediator mediator;
        private readonly ApplicationSettings applicationSettings;

        public CheckFunctionFilter(IMediator mediator, ApplicationSettings applicationSettings)
        {
            this.mediator = mediator;
            this.applicationSettings = applicationSettings;
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
                await mediator.Send(new PostCommand {
                    Name = functionName,
                    Path = path,
                    AccessToken = Guid.NewGuid().ToString("N")
                }, CancellationToken.None);

                await next();
            }

            if(function!= null && context.HttpContext.Request.Headers.TryGetValue("x-functions-key", out var functionsKey)
                && functionsKey == function.AccessToken)
            {
                await next();
            }

            context.Result = new UnauthorizedObjectResult(new Models.Response(StatusCodes.Status401Unauthorized, "Unauthorised function call"));
        }
    }
}
