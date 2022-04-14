using DiabetesManagement.Api.Base;
using DiabetesManagement.Contracts;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.User;
using DiabetesManagement.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace DiabetesManagement.Api.Features.User
{
    public class Api : ApiBase
    {
        public const string BaseUrl = "user";
        public Api(IConvertorFactory convertorFactory, IMediator mediator) : base(convertorFactory, mediator)
        {
        }

        [FunctionName("Register-user")]
        public async Task<IActionResult> Register(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/register")]
        HttpRequest request)
        {
            try
            {
                var result = await Mediator.Send(request.Form.Bind<PostCommand>(ConvertorFactory));
                return new OkObjectResult(new Response(result));
            }
            catch (InvalidOperationException ex)
            {
                return new BadRequestObjectResult(new Response(400, ex.Message));
            }
            
        }

        [FunctionName("Login-user")]
        public async Task<IActionResult> Login(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/login")]
        HttpRequest request)
        {
            var getRequest = request.Form.Bind<GetRequest>(ConvertorFactory);
            getRequest.AuthenticateUser = true;
            var result = await Mediator.Send(getRequest);
            try
            {
                if (result == null)
                {
                    throw new InvalidOperationException("Invalid username or password");
                }

                return new OkObjectResult(new Response(result));
            }
            catch(InvalidOperationException ex)
            {
                return new UnauthorizedObjectResult(new Response(401, ex.Message));
            }
        }
    }
}
