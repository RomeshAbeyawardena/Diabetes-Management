using Inventory.Features.AccessToken;
using Inventory.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Features.AccessToken
{
    [Route(ApiUrl)]
    public class Api : ApiBase
    {
        public const string ApiUrl = $"{BaseUrl}/access-token";
        public Api(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] SignRequest request, CancellationToken cancellationToken)
        {
            return await Handle(request, cancellationToken);
        }
    }
}
