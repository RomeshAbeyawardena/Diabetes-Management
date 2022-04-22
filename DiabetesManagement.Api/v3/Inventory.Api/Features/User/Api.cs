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
            return await TryHandler(async (ct) => await Mediator
                .Send(request.Form.Bind<PostCommand>(ConvertorFactory), ct), cancellationToken);
        }

        [FunctionName("login-user")]
        public async Task<IActionResult> Login(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/login")]
        HttpRequest request, CancellationToken cancellationToken)
        {
            var getRequest = request.Form.Bind<GetRequest>(ConvertorFactory);
            getRequest.AuthenticateUser = true;
            return await TryHandler(async (ct) =>
            {
                var result = await Mediator.Send(getRequest);
                object finalResult;

                if (result == null)
                {
                    throw new UnauthorizedAccessException("Invalid username or password");
                }

                //check for existing session
                var session = await Mediator.Send(new SessionFeature.GetRequest { UserId = result.UserId }, cancellationToken);

                finalResult = await Mediator.Send(new SessionFeature.PostCommand
                {
                    SessionId = session?.SessionId,
                    UserId = result.UserId,
                }, cancellationToken);

                return finalResult;
            }, cancellationToken);
        }

        [FunctionName("logout-user")]
        public async Task<IActionResult> Logout(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/logout")]
        HttpRequest request, CancellationToken cancellationToken)
        {
            return await TryHandler(async (ct) =>
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
                });

                return expiredSession;
            }, cancellationToken);
        }
    }
}
