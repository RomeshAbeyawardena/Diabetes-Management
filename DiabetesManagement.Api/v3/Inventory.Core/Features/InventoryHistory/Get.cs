using MediatR;
using Inventory.Features.InventoryHistory;

namespace Inventory.Core.Features.InventoryHistory;

public class Get : IRequestHandler<GetRequest, IEnumerable<Models.InventoryHistory>>
{
    private readonly IInventoryHistoryRepository inventoryHistoryRepository;

    public Get(IInventoryHistoryRepository inventoryHistoryRepository)
    {
        this.inventoryHistoryRepository = inventoryHistoryRepository;
    }

    public Task<IEnumerable<Models.InventoryHistory>> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        return inventoryHistoryRepository.Get(request, cancellationToken);
    }
}
