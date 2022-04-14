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
    public Api(IMediator mediator, Contracts.IConvertorFactory convertorFactor) : base(convertorFactor, mediator)
    {
    }

    [FunctionName("Get-Inventory")]
    public async Task<IActionResult> GetInventory(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = BaseUrl)] 
        HttpRequest request)
    {
        var result = await Mediator.Send(request.Query.Bind<GetRequest>(ConvertorFactory));
        return new OkObjectResult(result);
    }

    [FunctionName("Save-Inventory")]
    public async Task<IActionResult> SaveInventory(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = BaseUrl)]
        HttpRequest request)
    {
        var result = await Mediator.Send(request.Form.Bind<PostCommand>(ConvertorFactory));
        return new OkObjectResult(result);
    }
}