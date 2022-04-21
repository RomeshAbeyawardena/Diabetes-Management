using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace DiabetesManagement.Api.Features.Inventory;

using DiabetesManagement.Api.Base;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.Inventory;
using InventoryHistoryFeature = DiabetesManagement.Features.InventoryHistory;

public class Api : ApiBase
{
    public const string BaseUrl = "inventory";
    private readonly IMapper mapper;

    public Api(IMediator mediator, Contracts.IConvertorFactory convertorFactor, IMapper mapper) : base(convertorFactor, mediator)
    {
        this.mapper = mapper;
    }

    [FunctionName("get-inventory")]
    public async Task<IActionResult> GetInventory(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = BaseUrl)] 
        HttpRequest request, CancellationToken cancellationToken)
    {
        var getRequest = request.Query.Bind<GetRequest>(ConvertorFactory);
        return await TryHandler(request, getRequest.UserId!.Value, async (ct) => 
            await Mediator.Send(getRequest, ct), cancellationToken);
    }

    [FunctionName("list-inventory")]
    public async Task<IActionResult> ListInventory(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = $"{BaseUrl}/list")]
        HttpRequest request, CancellationToken cancellationToken)
    {
        var getRequest = request.Query.Bind<GetRequest>(ConvertorFactory);

        return await TryHandler(request, getRequest.UserId!.Value, async (ct) =>
        {
            var result = await Mediator.Send(getRequest, ct);
            return mapper.Map<IEnumerable<Models.Version>>(result);
        }, cancellationToken);
    }

    [FunctionName("save-inventory")]
    public async Task<IActionResult> SaveInventory(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = BaseUrl)]
        HttpRequest request, CancellationToken cancellationToken)
    {
        var postRequest = request.Form.Bind<InventoryHistoryFeature.PostCommand>(ConvertorFactory);

        return await TryHandler(request, postRequest.UserId!.Value, async (ct) => await Mediator
            .Send(postRequest, ct), cancellationToken);
    }
}