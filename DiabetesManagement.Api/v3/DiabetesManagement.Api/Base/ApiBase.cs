﻿using DiabetesManagement.Contracts;
using DiabetesManagement.Features.Session;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiabetesManagement.Api.Base;
public abstract class ApiBase
{
    private readonly IConvertorFactory convertorFactory;
    private readonly IMediator mediator;

    protected IConvertorFactory ConvertorFactory => convertorFactory;
    protected IMediator Mediator => mediator;

    protected bool TryGetSessionId(HttpRequest httpRequest, out Guid sessionId)
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

    protected async Task<IActionResult> Validate(HttpRequest httpRequest, Guid userId, Func<Task<IActionResult>> successAction)
    {
        if (!await ValidateSession(httpRequest, userId))
        {
            return new UnauthorizedObjectResult(new Models.Response(StatusCodes.Status401Unauthorized, "Unauthorised request"));
        }

        return await successAction();
    }

    public ApiBase(IConvertorFactory convertorFactory, IMediator mediator)
    {
        this.convertorFactory = convertorFactory;
        this.mediator = mediator;
    }
}
