using DiabetesManagement.Api.Base;
using DiabetesManagement.Contracts;
using DiabetesManagement.Extensions.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SessionFeature = DiabetesManagement.Features.Session;
using UserFeature = DiabetesManagement.Features.User;

namespace DiabetesManagement.Api.Features.User
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
            try
            {
                var result = await Mediator.Send(request.Form.Bind<UserFeature.PostCommand>(ConvertorFactory), cancellationToken);
                return new OkObjectResult(new Models.Response(result));
            }
            catch (InvalidOperationException ex)
            {
                return new BadRequestObjectResult(new Models.Response(400, ex.Message));
            }

        }

        [FunctionName("login-user")]
        public async Task<IActionResult> Login(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/login")]
        HttpRequest request, CancellationToken cancellationToken)
        {
            var getRequest = request.Form.Bind<UserFeature.GetRequest>(ConvertorFactory);
            getRequest.AuthenticateUser = true;

            var result = await Mediator.Send(getRequest);
            object finalResult;
            try
            {
                if (result == null)
                {
                    throw new InvalidOperationException("Invalid username or password");
                }

                //check for existing session
                var session = await Mediator.Send(new SessionFeature.GetRequest { UserId = result.UserId }, cancellationToken);

                finalResult = await Mediator.Send(new SessionFeature.PostCommand
                {
                    SessionId = session?.SessionId,
                    UserId = result.UserId,
                }, cancellationToken);

                return new OkObjectResult(new Models.Response(finalResult));
            }
            catch (InvalidOperationException ex)
            {
                return new UnauthorizedObjectResult(new Models.Response(401, ex.Message));
            }
        }

        [FunctionName("logout-user")]
        public async Task<IActionResult> Logout(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/logout")]
        HttpRequest request)
        {
            var session = await GetSession(request);

            if(session == null)
            {
                return new BadRequestObjectResult(new Models.Response(StatusCodes.Status400BadRequest, "Session invalid"));
            }

            var expiredSession = await Mediator.Send(new SessionFeature.PostCommand
            {
                SessionId = session?.SessionId,
                UserId = session?.UserId,
                ExpireSession = true
            });

            return new OkObjectResult(new Models.Response(expiredSession));
        }
    }
}
