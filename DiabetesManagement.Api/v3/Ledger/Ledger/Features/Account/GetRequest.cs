using Inventory.Attributes;
using MediatR;

namespace Ledger.Features.Account;
[RequiresClaims(Permissions.Account_View)]
public class GetRequest : IRequest<Models.Account>
{
    public Guid? AccountId { get; set; }
    public string? Reference { get; set; }
}
