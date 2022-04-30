using Inventory.Attributes;
using MediatR;

namespace Ledger.Features.Account;
[RequiresClaims(Permissions.Account_Edit)]
public class PostCommand : IRequest<Models.Account>
{
    public string? Reference { get; set; }
}
