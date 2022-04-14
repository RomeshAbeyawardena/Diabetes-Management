using DiabetesManagement.Features.Inventory;
using MediatR;

namespace DiabetesManagement.Core.Features.Inventory;

public class Get : IRequestHandler<GetRequest, IEnumerable<Models.InventoryHistory>>
{
    private readonly IInventoryRepository inventoryRepository;

    public Get(IInventoryRepository inventoryRepository)
    {
        this.inventoryRepository = inventoryRepository;
    }

    public Task<IEnumerable<Models.InventoryHistory>> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
