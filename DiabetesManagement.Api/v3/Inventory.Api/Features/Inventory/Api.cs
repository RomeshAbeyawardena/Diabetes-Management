using AutoMapper;
using Inventory.Api.Base;
using Inventory.Contracts;
using Inventory.Features.InventoryHistory;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Inventory.Api.Features.Inventory;

public class Api : ApiBase
{
    public const string ApiUrl = "inventory";
    private readonly IMapper mapper;

    public Api(IMediator mediator, IConvertorFactory convertorFactor, IMapper mapper) : base(convertorFactor, mediator)
    {
        this.mapper = mapper;
    }

    [FunctionName("get-inventory")]
    public async Task<IActionResult> GetInventory(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = ApiUrl)]
        HttpRequest request, CancellationToken cancellationToken)
    {
        return await TryHandler<GetRequest, IEnumerable<Models.InventoryHistory>>(request, r => r.UserId!.Value, 
            async (getRequest, ct) => await Mediator.Send(getRequest, ct), cancellationToken);
    }

    [FunctionName("list-inventory")]
    public async Task<IActionResult> ListInventory(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = $"{ApiUrl}/list")]
        HttpRequest request, CancellationToken cancellationToken)
    {
        return await TryHandler<GetRequest, IEnumerable<Models.Version>>(request, r => r.UserId!.Value, async (getRequest, ct) =>
        {
            var result = await Mediator.Send(getRequest, ct);
            return mapper.Map<IEnumerable<Models.Version>>(result);
        }, cancellationToken);
    }

    [FunctionName("save-inventory")]
    public async Task<IActionResult> SaveInventory(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = ApiUrl)]
        HttpRequest request, CancellationToken cancellationToken)
    {
        return await TryHandler<PostCommand, Models.InventoryHistory>(request, c => c.UserId!.Value, async (postRequest, ct) => await Mediator
            .Send(postRequest, ct), cancellationToken, r => r.Form);
    }
}