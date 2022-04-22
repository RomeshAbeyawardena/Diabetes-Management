using MediatR;

namespace Inventory.Features.ApplicationInstance;

public class SaveCommand : IRequest<Models.ApplicationInstance>
{
    public Models.ApplicationInstance? ApplicationInstance { get; set; }
    public bool CommitChanges { get; set; }
}
