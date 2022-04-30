using MediatR;

namespace Ledger.Features.Account;

public class PostCommand : IRequest<Models.Account>
{
    public string? Reference { get; set; }
}
