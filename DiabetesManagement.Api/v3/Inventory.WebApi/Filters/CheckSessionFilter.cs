using Inventory.Features;
using Inventory.Features.Session;
using Inventory.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inventory.WebApi.Filters;

public class CheckSessionFilter : IAsyncActionFilter, IDisposable
{
    void IDisposable.Dispose()
    {
        cancellationTokenSource.Dispose();
        GC.SuppressFinalize(this);
    }

    private readonly IMediator mediator;
    private readonly CancellationTokenSource cancellationTokenSource;
    public CheckSessionFilter(IMediator mediator)
    {
        this.mediator = mediator;
        cancellationTokenSource = new();
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var items = context.HttpContext.Items;

        //Determine whether the action requires session validation
        if (!items.TryGetValue(ValidateSessionAttribute.ValidateSessionKey, out var value) || (bool)value! == false)
        {
            //We aren't validating this session
            await next();
            return;
        }

        var headers = context.HttpContext.Request.Headers;
        //retrieve session key from headers
        if (headers.TryGetValue(Keys.SessionTokenKey, out var sessionTokenKey))
        {
            if (items.TryGetValue(ValidateSessionAttribute.UserIdValueKey, out var entry) )
            {
                var session = await mediator.Send(new GetRequest { 
                    AccessToken = sessionTokenKey, 
                    UserId = (Guid)entry!
                }, cancellationTokenSource.Token);

                if(session != null)
                {
                    await next();
                    return;
                }
            }
        }

        context.Result = new UnauthorizedObjectResult(new Models.Response(StatusCodes.Status401Unauthorized, "Session invalid"));
    }
}
