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

        try
        {
            var result = await action(cancellationToken);
            return Ok(new Models.Response(result));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new Models.Response(StatusCodes.Status406NotAcceptable, exception.Message));
        }
        catch (ValidationException exception)
        {
            return BadRequest(new Models.Response(StatusCodes.Status400BadRequest, exception.Message));
        }
        catch (InvalidDataException exception)
        {
            return BadRequest(new Models.Response(StatusCodes.Status400BadRequest, exception.Message));
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new Models.Response(StatusCodes.Status401Unauthorized, exception.Message));
        }
        catch (Exception exception)
        {
            return UnprocessableEntity(new Models.Response(StatusCodes.Status500InternalServerError, exception.Message));
        }
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
