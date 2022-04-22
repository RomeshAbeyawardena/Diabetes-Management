using Inventory.Contracts;
using MediatR;

namespace Inventory.Features.ApplicationInstance;

public class SaveCommand : IRequest<Models.ApplicationInstance>, ITransactionalCommand<Models.ApplicationInstance>
{
    Models.ApplicationInstance ITransactionalCommand<Models.ApplicationInstance>.Model => ApplicationInstance!;
    public Models.ApplicationInstance? ApplicationInstance { get; set; }
    public bool CommitChanges { get; set; }
}
