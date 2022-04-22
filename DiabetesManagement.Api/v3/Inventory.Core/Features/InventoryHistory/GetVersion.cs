using Inventory.Features.InventoryHistory;
using MediatR;

namespace Inventory.Core.Features.InventoryHistory;

public class GetVersion : IRequestHandler<GetVersionRequest, int>
{
    private readonly IInventoryHistoryRepository inventoryRepository;

    public GetVersion(IInventoryHistoryRepository inventoryRepository)
    {
        this.inventoryRepository = inventoryRepository;
    }

    public Task<int> Handle(GetVersionRequest request, CancellationToken cancellationToken)
    {
        return inventoryRepository.GetLatestVersion(request, cancellationToken);
    }
}
