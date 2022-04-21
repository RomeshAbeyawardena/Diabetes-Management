using DiabetesManagement.Features.InventoryHistory;
using MediatR;

namespace DiabetesManagement.Core.Features.InventoryHistory;

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
