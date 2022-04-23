using Inventory.Api.Base;
using Inventory.Contracts;
using Inventory.Extensions;
using Inventory.Features.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SessionFeature = Inventory.Features.Session;
using UserFeature = Inventory.Features.User;

namespace Inventory.Api.Features.User
{
    public class Api : ApiBase
    {
        public const string BaseUrl = "user";
        public Api(IConvertorFactory convertorFactory, IMediator mediator) : base(convertorFactory, mediator)
        {
        }

        [FunctionName("register-user")]
        public async Task<IActionResult> Register(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/register")]
        HttpRequest request, CancellationToken cancellationToken)
        {
            return await TryHandler<PostCommand, Models.User>(request, async (command, ct) => await Mediator
                .Send(command, ct), cancellationToken, r => r.Form);
        }

        [FunctionName("login-user")]
        public async Task<IActionResult> Login(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/login")]
        HttpRequest request, CancellationToken cancellationToken)
        {
            return await TryHandler<GetRequest, Models.Session>(request, async (getRequest, ct) =>
            {
                getRequest.AuthenticateUser = true;
                var result = await Mediator.Send(getRequest, ct);

                if (result == null)
                {
                    throw new UnauthorizedAccessException("Invalid username or password");
                }

                //check for existing session
                var session = await Mediator.Send(new SessionFeature.GetRequest { UserId = result.UserId }, cancellationToken);

                return await Mediator.Send(new SessionFeature.PostCommand
                {
                    SessionId = session?.SessionId,
                    UserId = result.UserId,
                }, cancellationToken);
            }, cancellationToken, r => r.Form);
        }

        [FunctionName("logout-user")]
        public async Task<IActionResult> Logout(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/logout")]
        HttpRequest request, CancellationToken cancellationToken)
        {
            return await TryHandler<object, Models.Session>(request, async (model, ct) =>
            {
                var session = await GetSession(request);

                if (session == null)
                {
                    throw new InvalidOperationException("Session invalid");
                }

                var expiredSession = await Mediator.Send(new SessionFeature.PostCommand
                {
                    SessionId = session?.SessionId,
                    ExpireSession = true
                }, ct);

                return expiredSession;
            }, cancellationToken);
        }
    }
}
