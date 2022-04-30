using MediatR;

namespace Ledger.Features.Account;

public class GetRequest : IRequest<Models.Account>
{
    public Guid? AccountId { get; set; }
    public string? Reference { get; set; }
}
