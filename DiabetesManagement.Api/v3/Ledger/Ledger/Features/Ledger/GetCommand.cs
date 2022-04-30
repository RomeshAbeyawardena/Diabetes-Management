using MediatR;

namespace Ledger.Features.Ledger;

public class GetCommand : IRequest<IEnumerable<Models.Ledger>>
{

}
