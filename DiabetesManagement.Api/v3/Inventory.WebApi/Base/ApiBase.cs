using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Inventory.WebApi.Base;

[ApiController, Route(BaseUrl)]
public abstract class ApiBase : ControllerBase
{
    private readonly IMediator mediator;
    protected const string BaseUrl = "/api"; 

    protected IMediator Mediator => mediator;
    
    protected async Task<IActionResult> Handle<TResult>(Func<CancellationToken, Task<TResult>> action, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Models.Response(ModelState));
        }

        var result = await action(cancellationToken);
        return Ok(new Models.Response(result));       
    }

    protected async Task<IActionResult> Handle<TRequest>(TRequest request, CancellationToken cancellationToken)
    {
        return await Handle(async (ct) => await mediator.Send(request!, ct), cancellationToken);
    }

    public ApiBase(IMediator mediator)
    {
        this.mediator = mediator;
    }
}
