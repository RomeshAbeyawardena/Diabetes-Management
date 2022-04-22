using MediatR;

namespace Inventory.Features.User;

public class PostCommand : IRequest<Models.User>
{
    public string? EmailAddress { get; set; }
    public string? DisplayName { get; set; }
    public string? Password { get; set; }
}
