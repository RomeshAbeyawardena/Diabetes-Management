using Inventory.Api.Base;
using Inventory.Contracts;
using Inventory.Extensions;
using Inventory.Features.AccessToken;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Inventory.Api.Features.AccessToken;

public class Api : ApiBase
{
    public const string BaseUrl = "access-token";
    public Api(IConvertorFactory convertorFactory, IMediator mediator) : base(convertorFactory, mediator)
    {
    }

    [FunctionName("Sign")]
    public async Task<IActionResult> Sign([HttpTrigger(AuthorizationLevel.Function, "POST", Route = BaseUrl)]
        HttpRequest request, CancellationToken cancellationToken)
    {
        return await TryHandler(async (ct) => await Mediator
            .Send(request.Form.Bind<SignRequest>(ConvertorFactory), ct), cancellationToken);
    }
}
