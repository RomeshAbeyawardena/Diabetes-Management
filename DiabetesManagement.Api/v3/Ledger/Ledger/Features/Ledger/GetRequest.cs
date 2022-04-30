using MediatR;

namespace Ledger.Features.Ledger;

public class GetRequest : IRequest<IEnumerable<Models.Ledger>>
{

}
