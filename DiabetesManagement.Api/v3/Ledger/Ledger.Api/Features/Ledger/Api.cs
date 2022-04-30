using Inventory.WebApi.Base;
using Ledger.Features.Ledger;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ledger.Api.Features.Ledger;

public class Api : ApiBase
{
    public const string ApiUrl = $"{BaseUrl}/ledger";
    public Api(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public Task<IActionResult> Get(GetRequest request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }

    [HttpPost]
    public Task<IActionResult> Post(PostCommand command, CancellationToken cancellationToken)
    {
        return Handle(command, cancellationToken);
    }
}
