using MediatR;
using Ledger.Features.Ledger;

namespace Ledger.Core.Features.Ledger;

public class Post : IRequestHandler<PostCommand, Models.Ledger>
{
    private readonly ILedgerRepository ledgerRepository;

    public Post(ILedgerRepository ledgerRepository)
    {
        this.ledgerRepository = ledgerRepository;
    }

    public async Task<Models.Ledger> Handle(PostCommand request, CancellationToken cancellationToken)
    {

        var previousLedger = await ledgerRepository.GetPreviousLedger(new GetRequest { AccountId = request.AccountId }, cancellationToken);
        var balance = previousLedger?.Balance ?? 0;
        return await ledgerRepository.SaveLedger(new SaveCommand
        {
            Ledger = new()
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                PreviousBalance = balance,
                Balance = balance + request.Amount,
                Reference = request.Reference,
            }
        }, cancellationToken);
    }
}
