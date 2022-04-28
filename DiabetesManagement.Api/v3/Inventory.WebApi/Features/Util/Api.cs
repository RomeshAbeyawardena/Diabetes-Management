
using Inventory.Features.Jwt;
using Inventory.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Features.Util;

[Route(ApiUrl)]
public class Api : ApiBase
{
    public const string ApiUrl = $"{BaseUrl}/util";

    public Api(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost, Route("encode")]
    public async Task<IActionResult> Encode([FromBody] SignRequest request, CancellationToken cancellationToken)
    {
        return await Handle(request, cancellationToken);
    }

    [HttpPost, Route("decode")]
    public async Task<IActionResult> Decode([FromForm] DecodeRequest request, CancellationToken cancellationToken)
    {
        return await Handle(request, cancellationToken);
    }
}
