using Inventory.WebApi.Base;
using MediatR;

namespace Ledger.Api.Features.Account;

public class Api : ApiBase
{
    public const string ApiUrl = $"{BaseUrl}/account";
    public Api(IMediator mediator) : base(mediator)
    {
    }
}
