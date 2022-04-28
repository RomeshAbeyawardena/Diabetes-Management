using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Inventory.WebApi.Base;

[ApiController, Route(BaseUrl)]
public class ApiBase : ControllerBase
{
    private readonly IMediator mediator;
    protected const string BaseUrl = "/api"; 
    protected async Task<IActionResult> Handle<TRequest>(TRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Models.Response(ModelState));
        }

        try
        {
            var result = await mediator.Send(request!, cancellationToken);

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

    public ApiBase(IMediator mediator)
    {
        this.mediator = mediator;
    }
}
