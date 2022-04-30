using Inventory.Attributes;
using MediatR;

namespace Ledger.Features.Ledger;

[RequiresClaims(Permissions.Ledger_View)]
public class GetRequest : IRequest<IEnumerable<Models.Ledger>>
{
    public Guid? LedgerId { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountReference { get; set; }
    public string? LedgerReference { get; set; }
}
