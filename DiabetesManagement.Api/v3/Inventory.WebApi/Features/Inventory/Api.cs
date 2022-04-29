using AutoMapper;
using Inventory.Features.InventoryHistory;
using Inventory.WebApi.Attributes;
using Inventory.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Features.Inventory;

[Route(ApiUrl)]
public class Api : ApiBase
{
    public const string ApiUrl = $"{BaseUrl}/inventory";
    private readonly IMapper mapper;

    public Api(IMapper mapper, IMediator mediator) : base(mediator)
    {
        this.mapper = mapper;
    }

    [HttpGet, ValidateSession]
    public async Task<IActionResult> Get([FromQuery] GetRequest request, CancellationToken cancellationToken)
    {
        return await Handle(request, cancellationToken);
    }

    [HttpGet, Route("list"), ValidateSession]
    public async Task<IActionResult> List([FromQuery] GetRequest request, CancellationToken cancellationToken)
    {
        return await Handle(async(ct) =>
        {
            var result = await Mediator.Send(request, cancellationToken);
            return mapper.Map<IEnumerable<Models.Version>>(result);
        }, cancellationToken);
    }

    [HttpPost, ValidateSession]
    public async Task<IActionResult> Save([FromForm] PostCommand command, CancellationToken cancellationToken)
    {
        return await Handle(command, cancellationToken);
    }
}
