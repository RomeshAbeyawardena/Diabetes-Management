using MediatR;
using Ledger.Features.Account;

namespace Ledger.Core.Features.Account;

public class Get : IRequestHandler<GetRequest, Models.Account>
{
    private readonly IAccountRepository accountRepository;

    public Get(IAccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }

    public Task<Models.Account> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        return accountRepository.Get(request, cancellationToken);
    }
}
