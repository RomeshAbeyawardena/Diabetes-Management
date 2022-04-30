using MediatR;
using Ledger.Features.Ledger;

namespace Ledger.Core.Features.Ledger;

public class Get : IRequestHandler<GetRequest, IEnumerable<Models.Ledger>>
{
    private readonly ILedgerRepository ledgerRepository;

    public Get(ILedgerRepository ledgerRepository)
    {
        this.ledgerRepository = ledgerRepository;
    }

    public Task<IEnumerable<Models.Ledger>> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        return ledgerRepository.Get(request, cancellationToken);
    }
}
