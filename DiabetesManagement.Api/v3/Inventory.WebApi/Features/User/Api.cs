using Inventory.Features.User;
using Inventory.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SessionFeature = Inventory.Features.Session;

namespace Inventory.WebApi.Features.User;

[Route(ApiUrl)]
public class Api : ApiBase
{
    public const string ApiUrl = $"{BaseUrl}/user";
    public Api(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost, Route("register")]
    public async Task<IActionResult> Post([FromForm] PostCommand command, CancellationToken cancellationToken)
    {
        return await Handle(command, cancellationToken);
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Post([FromForm] GetRequest command, CancellationToken cancellationToken)
    {
        return await Handle(async(ct) => {
            var loginResult = await Mediator.Send(command, ct);

            if (loginResult == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var session = await Mediator.Send(new SessionFeature.GetRequest { UserId = loginResult.UserId }, ct);

            return await Mediator.Send(new SessionFeature.PostCommand
            {
                SessionId = session?.SessionId,
                UserId = loginResult.UserId,
            }, ct); }, cancellationToken); 
    }
}
