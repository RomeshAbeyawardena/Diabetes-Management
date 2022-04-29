using MediatR;

namespace Inventory.Features.Function;

public class PostCommand : IRequest<Models.Function>
{
    public string? Name { get; set; }
    public string? AccessToken { get; set; }
    public string? Path { get; set; }
}
