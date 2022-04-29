using Inventory.Features.Application;
using Inventory.WebApi.Attributes;
using Inventory.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Features.Application;
[Route(ApiUrl)]
public class Api : ApiBase
{
    public const string ApiUrl = $"{BaseUrl}/application";
    public Api(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpPost, ValidateSession]
    public async Task<IActionResult> Post([FromForm] PostCommand command, CancellationToken cancellationToken)
    {
        return await Handle(command, cancellationToken);
    }
}
