using Inventory.Api.Base;
using Inventory.Contracts;
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
        return await TryHandler<SignRequest, string>(request, async (signRequest, ct) => await Mediator
            .Send(signRequest, ct), cancellationToken, r => r.Form);
    }
}
