using MediatR;

namespace DiabetesManagement.Api.Base;
public abstract class ApiBase
{
    private readonly IMediator mediator;

    protected IMediator Mediator => mediator;

    public ApiBase(IMediator mediator)
    {
        this.mediator = mediator;
    }
}
