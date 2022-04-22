using Inventory.Features.Inventory;
using MediatR;

namespace Inventory.Core.Features.Inventory;

public class Post : IRequestHandler<PostCommand, Models.Inventory>
{
    private readonly IInventoryRepository inventoryRepository;

    public Post(IInventoryRepository inventoryRepository)
    {
        this.inventoryRepository = inventoryRepository;
    }

    public async Task<Models.Inventory> Handle(PostCommand request, CancellationToken cancellationToken)
    {
        return await inventoryRepository.Save(new SaveCommand
        {
            Inventory = request.Inventory ?? new Models.Inventory
            {
                DefaultIntent = request.DefaultIntent,
                Subject = request.Subject,
                UserId = request.UserId!.Value
            },
            CommitChanges = request.CommitChanges
        }, cancellationToken);
    }
}
