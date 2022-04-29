using Inventory.Features.Function;
using Inventory.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Features.Function;

public class Api : ApiBase
{
    public Api(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery]ListRequest request, CancellationToken cancellationToken)
    {
        return await Handle(request, cancellationToken);
    }
}
