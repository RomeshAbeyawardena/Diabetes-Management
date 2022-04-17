using DiabetesManagement.Api.Base;
using DiabetesManagement.Contracts;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.Application;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;

namespace DiabetesManagement.Api.Features.Application;

public class Api : ApiBase
{
    public const string BaseUrl = "application";
    public Api(IConvertorFactory convertorFactory, IMediator mediator) : base(convertorFactory, mediator)
    {
    }

    [FunctionName("save-application")]
    public async Task<IActionResult> SaveApplication(
        [HttpTrigger(Microsoft.Azure.WebJobs.Extensions.Http.AuthorizationLevel.Function, "POST", 
        Route = BaseUrl)]HttpRequest request, CancellationToken cancellationToken)
    {
        var command = request.Form.Bind<PostCommand>(ConvertorFactory);
        return new OkObjectResult(await Mediator.Send(command, cancellationToken));
    }
}
