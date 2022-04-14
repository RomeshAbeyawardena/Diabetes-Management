using DiabetesManagement.Contracts;
using MediatR;

namespace DiabetesManagement.Api.Base;
public abstract class ApiBase
{
    private readonly IConvertorFactory convertorFactory;
    private readonly IMediator mediator;

    protected IConvertorFactory ConvertorFactory => convertorFactory;
    protected IMediator Mediator => mediator;

    public ApiBase(IConvertorFactory convertorFactory, IMediator mediator)
    {
        this.convertorFactory = convertorFactory;
        this.mediator = mediator;
    }
}
