using DiabetesManagement.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiabetesManagement.Api.Base;
public abstract class ApiBase
{
    private readonly IConvertorFactory convertorFactory;
    private readonly IMediator mediator;

    protected IConvertorFactory ConvertorFactory => convertorFactory;
    protected IMediator Mediator => mediator;

    //protected async Task<IActionResult> HandleException(Func<Task> action, params Type[] exceptionTypes)
    //{  
    //    try
    //    {
    //        await action();
    //    }
    //    catch(Exception exception)
    //    {
    //        if (exceptionTypes.Contains(exception.GetType()))
    //        {

    //        }
    //    }
    //}


    public ApiBase(IConvertorFactory convertorFactory, IMediator mediator)
    {
        this.convertorFactory = convertorFactory;
        this.mediator = mediator;
    }
}
