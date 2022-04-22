using AutoMapper;
using InventoryFeature = DiabetesManagement.Features.Inventory;
using DiabetesManagement.Features.InventoryHistory;
using MediatR;

namespace DiabetesManagement.Core.Features.InventoryHistory;

public class Post : IRequestHandler<PostCommand, Models.InventoryHistory>
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly IInventoryHistoryRepository inventoryHistoryRepository;
    public Post(IMapper mapper, IMediator mediator, IInventoryHistoryRepository inventoryHistoryRepository)
    {
        this.mapper = mapper;
        this.mediator = mediator;
        this.inventoryHistoryRepository = inventoryHistoryRepository;
    }

    public async Task<Models.InventoryHistory> Handle(PostCommand request, CancellationToken cancellationToken)
    {
        var getRequest = mapper.Map<InventoryFeature.GetRequest>(request);
        
        var inventory = (await mediator.Send(getRequest, cancellationToken)).FirstOrDefault();
        var version = 1;
        //check if inventory exists;
        if (inventory == null)
        {
            //create inventory
            inventory = await mediator.Send(new InventoryFeature.PostCommand
            {
                    DefaultIntent = request.Type,
                    Subject = request.Key,
                    UserId = request.UserId!.Value,
                CommitChanges = false
            }, cancellationToken);
        }
        else
        {
            //increment version for new version
            version = await inventoryHistoryRepository.GetLatestVersion(mapper.Map<GetVersionRequest>(request), cancellationToken) + 1;
            //update modified flag
            await mediator.Send(new InventoryFeature.PostCommand { 
                Inventory = inventory, 
                CommitChanges = false 
            }, cancellationToken);
        }

        return await inventoryHistoryRepository.Save(new SaveCommand
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
