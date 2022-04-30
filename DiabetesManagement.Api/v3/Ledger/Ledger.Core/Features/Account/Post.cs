using MediatR;
using Ledger.Features.Account;

namespace Ledger.Core.Features.Account;

public class Post : IRequestHandler<PostCommand, Models.Account>
{
    private readonly IAccountRepository accountRepository;

    public Post(IAccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }

    public Task<Models.Account> Handle(PostCommand request, CancellationToken cancellationToken)
    {
        return accountRepository.SaveAccount(new SaveCommand { 
            Account = new() {
                Reference = request.Reference
            }, CommitChanges = true }, cancellationToken);

    }
}
