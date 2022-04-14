using AutoMapper;
using DiabetesManagement.Features.Inventory;
using DiabetesManagement.Features.InventoryHistory;
using MediatR;

namespace DiabetesManagement.Core.Features.Inventory;

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
        if(inventory == null)
        {
            inventory = await inventoryRepository.Save(new DiabetesManagement.Features.Inventory.SaveCommand
            {
                Inventory = new Models.Inventory
                {
                    DefaultType = request.Type,
                    Key = request.Key,
                    UserId = request.UserId!.Value
                },
                CommitChanges = false
            }, cancellationToken);
        }
        else
        {
            version = (await inventoryHistoryRepository.GetLatestVersion(getRequest, cancellationToken)) + 1;
        }

        return await inventoryHistoryRepository.Save(new DiabetesManagement.Features.InventoryHistory.SaveCommand
        {
            InventoryHistory = new Models.InventoryHistory
            {
                InventoryId = inventory.InventoryId,
                Items = request.Items,
                Type = request.Type,
                Version = version
            },
            CommitChanges = true
        }, cancellationToken);
    }
}
