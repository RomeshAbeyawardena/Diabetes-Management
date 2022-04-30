using MediatR;

namespace Ledger.Features.Ledger;

public class PostCommand : IRequest<Models.Ledger>
{
    public Guid AccountId { get; set; }
    public string? Reference { get; set; }
    public decimal Amount { get; set; }
}
