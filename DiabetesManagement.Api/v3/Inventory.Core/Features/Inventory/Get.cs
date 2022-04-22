using DiabetesManagement.Features.Inventory;
using MediatR;

namespace DiabetesManagement.Core.Features.Inventory;

public class Get : IRequestHandler<GetRequest, IEnumerable<Models.Inventory>>
{
    private readonly IInventoryRepository inventoryRepository;

    public Get(IInventoryRepository inventoryRepository)
    {
        this.inventoryRepository = inventoryRepository;
    }

    public Task<IEnumerable<Models.Inventory>> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        return inventoryRepository.Get(request, cancellationToken);
    }
}
