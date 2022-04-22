using Inventory;
using Inventory.Api.Features;
using Inventory.Features.Session;
using Inventory.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Base;

public abstract class ApiBase
{
    private readonly IConvertorFactory convertorFactory;
    private readonly IMediator mediator;

    protected IConvertorFactory ConvertorFactory => convertorFactory;
    protected IMediator Mediator => mediator;

    protected static bool TryGetSessionId(HttpRequest httpRequest, out Guid sessionId)
    {
        sessionId = default;
        return httpRequest.Headers.TryGetValue("X-API-SESSION-KEY", out var sessionValue)
            && Guid.TryParse(sessionValue.FirstOrDefault(), out sessionId);
    }

    protected async Task<Models.Session> GetSession(HttpRequest httpRequest)
    {
        if (TryGetSessionId(httpRequest, out var sessionId))
        {
            return await Mediator.Send(new GetRequest { SessionId = sessionId });
        }

        return null!;
    }

    protected async Task<bool> ValidateSession(HttpRequest httpRequest, Guid userId)
    {
        if (TryGetSessionId(httpRequest, out var sessionId))
        {
            var session = await Mediator.Send(new GetRequest
            {
                AuthenticateSession = true,
                SessionId = sessionId,
                UserId = userId
            });

            return session != null;
        }

        return false;
    }

    protected async Task<IActionResult> TryHandler<T>(HttpRequest httpRequest, Guid userId, Func<CancellationToken, Task<T>> attempt, CancellationToken cancellationToken)
    {
        if (!await ValidateSession(httpRequest, userId))
        {
            return new UnauthorizedObjectResult(new Models.Response(StatusCodes.Status401Unauthorized, "Unauthorised session"));
        }

        return await TryHandler(attempt, cancellationToken);
    }

    protected static async Task<IActionResult> TryHandler<T>(Func<CancellationToken, Task<T>> attempt, CancellationToken cancellationToken)
    {
        try
        {
            return new ObjectResult(new Models.Response(await attempt(cancellationToken)));
        }
        catch (InvalidOperationException exception)
        {
            return new BadRequestObjectResult(new Models.Response(StatusCodes.Status406NotAcceptable, exception.Message));
        }
        catch (InvalidDataException exception)
        {
            return new BadRequestObjectResult(new Models.Response(StatusCodes.Status400BadRequest, exception.Message));
        }
        catch (UnauthorizedAccessException exception)
        {
            return new UnauthorizedObjectResult(new Models.Response(StatusCodes.Status401Unauthorized, exception.Message));
        }
        catch (Exception exception)
        {
#if DEBUG
            throw;
#else
            return new UnprocessableEntityObjectResult(new Models.Response(StatusCodes.Status500InternalServerError, exception.Message));
#endif

        }
    }

    public ApiBase(IConvertorFactory convertorFactory, IMediator mediator)
    {
        this.convertorFactory = convertorFactory;
        this.mediator = mediator;
    }
}
