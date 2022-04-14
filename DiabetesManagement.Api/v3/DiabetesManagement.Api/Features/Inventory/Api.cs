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
using DiabetesManagement.Features.InventoryHistory;

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
        HttpRequest request)
    {
        var getRequest = request.Query.Bind<GetRequest>(ConvertorFactory);

        return await Validate(request, getRequest.UserId!.Value, async () =>
        {
            var result = await Mediator.Send(getRequest);
            return new OkObjectResult(new Models.Response(result));
        });
    }

    [FunctionName("list-inventory")]
    public async Task<IActionResult> ListInventory(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = $"{BaseUrl}/list")]
        HttpRequest request)
    {
        var getRequest = request.Query.Bind<GetRequest>(ConvertorFactory);

        return await Validate(request, getRequest.UserId!.Value, async () =>
        {
            var result = await Mediator.Send(getRequest);
            var versions = mapper.Map<IEnumerable<Models.Version>>(result);

            return new OkObjectResult(new Models.Response(versions));
        });
        

    }

    [FunctionName("save-inventory")]
    public async Task<IActionResult> SaveInventory(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = BaseUrl)]
        HttpRequest request)
    {
        var postRequest = request.Form.Bind<PostCommand>(ConvertorFactory);

        return await Validate(request, postRequest.UserId!.Value, async () =>
        {
            var result = await Mediator.Send(postRequest);
            return new OkObjectResult(new Models.Response(result));
        });
    }
}