using Inventory.Features.Function;
using Inventory.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Features.Function;

[Route(ApiUrl)]
public class Api : ApiBase
{
    public const string ApiUrl = $"{BaseUrl}/function";
    public Api(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery]ListRequest request, CancellationToken cancellationToken)
    {
        return await Handle(request, cancellationToken);
    }
}
