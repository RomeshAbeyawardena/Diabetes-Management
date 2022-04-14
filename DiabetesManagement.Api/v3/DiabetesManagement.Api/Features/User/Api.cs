using DiabetesManagement.Api.Base;
using DiabetesManagement.Contracts;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.User;
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
            var result = await Mediator.Send(request.Form.Bind<PostCommand>(ConvertorFactory));
            return new OkObjectResult(result);
        }

        [FunctionName("Login-user")]
        public async Task<IActionResult> Login(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/login")]
        HttpRequest request)
        {
            var getRequest = request.Form.Bind<GetRequest>(ConvertorFactory);
            getRequest.AuthenticateUser = true;
            var result = await Mediator.Send(getRequest);
            return new OkObjectResult(result);
        }
    }
}
