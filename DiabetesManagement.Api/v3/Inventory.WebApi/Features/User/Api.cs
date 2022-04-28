using Inventory.WebApi.Base;
using MediatR;

namespace Inventory.WebApi.Features.User;

public class Api : ApiBase
{
    public Api(IMediator mediator) : base(mediator)
    {
    }


}
