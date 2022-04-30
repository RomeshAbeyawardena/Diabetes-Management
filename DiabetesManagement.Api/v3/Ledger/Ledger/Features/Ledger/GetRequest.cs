using MediatR;

namespace Ledger.Features.Ledger;

public class GetRequest : IRequest<IEnumerable<Models.Ledger>>
{
    public Guid? LedgerId { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountReference { get; set; }
    public string? LedgerReference { get; set; }
}
