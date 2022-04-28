using Inventory.Api.Base;
using Inventory.Contracts;
using Inventory.Features.Jwt;
using Inventory.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Inventory.Api.Features.Util;

public class Api : ApiBase
{
    public const string BaseUrl = "util";

    public Api(IConvertorFactory convertorFactory, IMediator mediator) : base(convertorFactory, mediator)
    {
    }

    [FunctionName("encode")]
    public async Task<IActionResult> Encode([HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/encode")]
        HttpRequest request, CancellationToken cancellationToken)
    {
        var dict = request.Form.ToDictionary(k => k.Key, v => (object)v.Value);
        return new OkObjectResult(new Response(await Mediator.Send(new SignRequest { Dictionary = dict }, cancellationToken)));
    }

    [FunctionName("decode")]
    public async Task<IActionResult> Decode([HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/decode")]
        HttpRequest request, CancellationToken cancellationToken)
    {
        return await TryHandler<DecodeRequest, IDictionary<string, string>>(request, (r, ct) => Mediator.Send(r, ct), cancellationToken);
    }
}
