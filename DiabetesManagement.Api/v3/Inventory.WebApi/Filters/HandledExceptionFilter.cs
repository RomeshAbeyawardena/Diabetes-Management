using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace Inventory.WebApi.Filters
{
    public class HandledExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();
            
            if(exceptionType == typeof(UnauthorizedAccessException))
            {
                context.ExceptionHandled = true;
                context.Result = new UnauthorizedObjectResult(new Models.Response(StatusCodes.Status401Unauthorized, context.Exception.Message));
            }

            if(exceptionType == typeof(InvalidOperationException))
            {
                context.ExceptionHandled = true;
                context.Result = new BadRequestObjectResult(new Models.Response(StatusCodes.Status406NotAcceptable, context.Exception.Message));
            }

            if (exceptionType == typeof(ValidationException) || exceptionType == typeof(InvalidDataException))
            {
                context.ExceptionHandled = true;
                context.Result = new BadRequestObjectResult(new Models.Response(StatusCodes.Status400BadRequest, context.Exception.Message));
            }

            context.ExceptionHandled = true;
            context.Result = new UnprocessableEntityObjectResult(new Models.Response(StatusCodes.Status500InternalServerError, context.Exception.Message));
        }
    }
}
