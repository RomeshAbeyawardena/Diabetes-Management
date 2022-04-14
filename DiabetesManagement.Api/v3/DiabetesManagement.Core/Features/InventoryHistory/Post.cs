using AutoMapper;
using DiabetesManagement.Features.Inventory;
using DiabetesManagement.Features.InventoryHistory;
using MediatR;

namespace DiabetesManagement.Core.Features.InventoryHistory;

public class Post : IRequestHandler<PostCommand, Models.InventoryHistory>
{
    private readonly IMapper mapper;
    private readonly IInventoryHistoryRepository inventoryHistoryRepository;
    private readonly IInventoryRepository inventoryRepository;

    public Post(IMapper mapper, IInventoryHistoryRepository inventoryHistoryRepository, IInventoryRepository inventoryRepository)
    {
        this.mapper = mapper;
        this.inventoryHistoryRepository = inventoryHistoryRepository;
        this.inventoryRepository = inventoryRepository;
    }

    public async Task<Models.InventoryHistory> Handle(PostCommand request, CancellationToken cancellationToken)
    {
        var getRequest = mapper.Map<GetRequest>(request);
        //check if inventory exists;
        var inventory = (await inventoryRepository.Get(getRequest, cancellationToken)).SingleOrDefault();
        var version = 1;
        //create inventory
        if (inventory == null)
        {
            inventory = await inventoryRepository.Save(new DiabetesManagement.Features.Inventory.SaveCommand
            {
                Inventory = new Models.Inventory
                {
                    DefaultIntent = request.Type,
                    Subject = request.Key,
                    UserId = request.UserId!.Value
                },
                CommitChanges = false
            }, cancellationToken);
        }
        else
        {
            //increment version for new item
            version = await inventoryHistoryRepository.GetLatestVersion(getRequest, cancellationToken) + 1;
            //set modified date
            inventory.Modified = DateTimeOffset.UtcNow;
        }

        return await inventoryHistoryRepository.Save(new DiabetesManagement.Features.InventoryHistory.SaveCommand
        {
            InventoryHistory = new Models.InventoryHistory
            {
                InventoryId = inventory.InventoryId,
                Value = request.Items,
                Intent = request.Type,
                Version = version
            },
            CommitChanges = true
        }, cancellationToken);
    }
}
