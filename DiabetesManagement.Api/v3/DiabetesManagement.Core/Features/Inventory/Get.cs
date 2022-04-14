using DiabetesManagement.Features.Inventory;
using MediatR;

namespace DiabetesManagement.Core.Features.Inventory;

public class Get : IRequestHandler<GetRequest, IEnumerable<Models.InventoryHistory>>
{
    public Get(IInventoryRepository inventoryRepository)
    {

    }

    public Task<IEnumerable<Models.InventoryHistory>> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
