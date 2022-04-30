using MediatR;
using Ledger.Features.Account;

namespace Ledger.Core.Features.Account;

public class Get : IRequestHandler<GetRequest, Models.Account>
{
    public Task<Models.Account> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
