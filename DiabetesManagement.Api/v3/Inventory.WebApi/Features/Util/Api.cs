
using Inventory.Contracts;
using Inventory.Extensions;
using Inventory.Features.Jwt;
using Inventory.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Features.Util;

[Route(ApiUrl)]
public class Api : ApiBase
{
    public const string ApiUrl = $"{BaseUrl}/util";
    private readonly IConvertorFactory convertorFactory;

    public Api(IMediator mediator, IConvertorFactory convertorFactory) : base(mediator)
    {
        this.convertorFactory = convertorFactory;
    }

    [HttpPost, Route("encode")]
    public async Task<IActionResult> Encode([FromBody] SignRequest request, CancellationToken cancellationToken)
    {
        request.Dictionary = request.Values!.ToDictionary(k => k.Key, v => convertorFactory.Convert(v.Value));

        return await Handle(request, cancellationToken);
    }

    [HttpPost, Route("decode")]
    public async Task<IActionResult> Decode([FromForm] DecodeRequest request, CancellationToken cancellationToken)
    {
        return await Handle(request, cancellationToken);
    }
}
