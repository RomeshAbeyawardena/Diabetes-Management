using MediatR;
using Ledger.Features.Account;

namespace Ledger.Core.Features.Account;

public class Post : IRequestHandler<PostCommand, Models.Account>
{
    public Task<Models.Account> Handle(PostCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
