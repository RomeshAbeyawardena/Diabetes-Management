using MediatR;

namespace Inventory.Features.ApplicationInstance;

public class PostCommand : IRequest<Models.ApplicationInstance>
{
    public Guid ApplicationId { get; set; }
    public DateTimeOffset Expires { get; set; }
}
