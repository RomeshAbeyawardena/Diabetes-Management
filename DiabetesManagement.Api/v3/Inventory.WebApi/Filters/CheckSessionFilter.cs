using Inventory.Features;
using Inventory.Features.Session;
using Inventory.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Inventory.WebApi.Filters;

public class CheckSessionFilter : IAsyncActionFilter
{
    private readonly IMediator mediator;

    public CheckSessionFilter(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Items.TryGetValue(ValidateSessionAttribute.ValidateSessionKey, out var value) || (bool)value! == false)
        {
            await next();
            return;
        }

        var headers = context.HttpContext.Request.Headers;
        if (headers.TryGetValue(Keys.SessionTokenKey, out var sessionTokenKey) 
            && Guid.TryParse(sessionTokenKey, out var sessionTokenId))
        {
            if (context.ModelState.TryGetValue("userId", out var entry) 
                && Guid.TryParse(entry.RawValue!.ToString(), out var userId))
            {
                var session = await mediator.Send(new GetRequest { 
                    SessionId = sessionTokenId, 
                    UserId = userId }, CancellationToken.None);

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
