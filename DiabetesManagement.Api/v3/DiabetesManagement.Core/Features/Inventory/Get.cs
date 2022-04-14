using DiabetesManagement.Features.Inventory;
using MediatR;

namespace DiabetesManagement.Core.Features.Inventory;

public class Get : IRequestHandler<GetRequest, IEnumerable<Models.InventoryHistory>>
{
    private readonly IInventoryHistoryRepository inventoryRepository;

    public Get(IInventoryHistoryRepository inventoryRepository)
    {
        this.inventoryRepository = inventoryRepository;
    }

    public Task<IEnumerable<Models.InventoryHistory>> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        return inventoryRepository.Get(request, cancellationToken);
    }
}
