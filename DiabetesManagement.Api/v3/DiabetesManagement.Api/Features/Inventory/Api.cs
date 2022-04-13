using DiabetesManagement.Api.Base;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace DiabetesManagement.Api.Features.Inventory;
public class Api : ApiBase
{
    public const string BaseUrl = "inventory";
    public Api(IMediator mediator) : base(mediator)
    {
    }

    [FunctionName("Get-Inventory")]
    public async Task<IActionResult> GetInventory(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = BaseUrl)] 
        HttpRequest request)
    {
        return new OkResult();
    }
}