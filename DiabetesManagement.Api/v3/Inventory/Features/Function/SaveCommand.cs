using Inventory.Contracts;
using MediatR;

namespace Inventory.Features.Function;

public class SaveCommand : IRequest<Models.Function>, ITransactionalCommand<Models.Function>
{
    Models.Function? ITransactionalCommand<Models.Function>.Model => Function;
    public Models.Function? Function { get; set; }
    public bool CommitChanges { get; set; }
}
