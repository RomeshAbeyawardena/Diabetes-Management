using Inventory;
using Inventory.Api.Features;
using Inventory.Features.Session;
using Inventory.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Primitives;
using Inventory.Extensions;

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



    /// <summary>
    /// A try handler that expects a valid user session in order to proceed, calls the standard TryHandler once the user session is validated
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="httpRequest"></param>
    /// <param name="userId"></param>
    /// <param name="attempt"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task<IActionResult> TryHandler<T, TResult>(HttpRequest httpRequest, 
        Func<T,Guid> getUserId, Func<T, CancellationToken, Task<TResult>> attempt, 
        CancellationToken cancellationToken, 
        Func<HttpRequest, IEnumerable<KeyValuePair<string, StringValues>>>? keyValue = default)
        where T : notnull
    {
        return await TryHandler<T, TResult>(httpRequest, async(t, ct) => {
            var userId = getUserId(t);
            if (!await ValidateSession(httpRequest, userId))
            {
               throw new UnauthorizedAccessException("Unauthorised session");
            }
            return await attempt(t, ct);
        }, cancellationToken, keyValue);
    }

    /// <summary>
    /// A try handler that will handle exceptions for the below, returning status code 400 for all except a few
    /// <list type="bullet">
    /// <item>InvalidOperationException</item>
    /// <item>ValidationException</item>
    /// <item>InvalidDataException</item>
    /// <item>UnauthorizedAccessException (Unauthorised Object result)</item>
    /// <item>Falls back to UnprocessableEntityObjectResult if the exception is not in this list</item>
    /// </list>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="attempt"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task<IActionResult> TryHandler<T, TResult>(HttpRequest httpRequest, 
        Func<T, CancellationToken, Task<TResult>> attempt, 
        CancellationToken cancellationToken, 
        Func<HttpRequest, IEnumerable<KeyValuePair<string, StringValues>>>? keyValue = default)
        where T : notnull
    {
        if(keyValue == default)
        {
            keyValue = r => r.Query;
        }

        try
        {
            var value = keyValue(httpRequest).Bind<T>(ConvertorFactory);

            return new ObjectResult(new Models.Response(await attempt(value, cancellationToken)));
        }
        catch (InvalidOperationException exception)
        {
            return new BadRequestObjectResult(new Models.Response(StatusCodes.Status406NotAcceptable, exception.Message));
        }
        catch (ValidationException exception)
        {
            return new BadRequestObjectResult(new Models.Response(StatusCodes.Status400BadRequest, exception.Message));
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
            return new UnprocessableEntityObjectResult(new Models.Response(StatusCodes.Status500InternalServerError, exception.Message));
        }
    }

    public ApiBase(IConvertorFactory convertorFactory, IMediator mediator)
    {
        this.convertorFactory = convertorFactory;
        this.mediator = mediator;
    }
}
