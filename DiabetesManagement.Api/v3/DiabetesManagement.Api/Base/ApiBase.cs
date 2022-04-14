using DiabetesManagement.Contracts;
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

    protected async Task<bool> ValidateSession(HttpRequest httpRequest, Guid userId)
    {
        if(httpRequest.Headers.TryGetValue("X-API-SESSION-KEY", out var sessionValue) 
            && Guid.TryParse(sessionValue.FirstOrDefault(), out var sessionId))
        {
            var session = await Mediator.Send(new GetRequest { AuthenticateSession = true, SessionId = sessionId, UserId = userId });

            return session != null;
        }

        return false;
    }

    public ApiBase(IConvertorFactory convertorFactory, IMediator mediator)
    {
        this.convertorFactory = convertorFactory;
        this.mediator = mediator;
    }
}
