using Inventory.Attributes;
using MediatR;

namespace Ledger.Features.Ledger;
[RequiresClaims(Permissions.Ledger_Edit)]
public class PostCommand : IRequest<Models.Ledger>
{
    public Guid AccountId { get; set; }
    public string? Reference { get; set; }
    public decimal Amount { get; set; }
}
