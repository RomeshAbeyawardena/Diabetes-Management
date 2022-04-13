using DiabetesManagement.Shared.RequestHandlers.Inventory;
namespace DiabetesManagement.Shared.Contracts
{
    public interface IInventoryRepository : IRepository<Models.Inventory>
    {
        Task<IEnumerable<Models.Inventory>> Get(GetRequest request, CancellationToken cancellationToken);
        Task<Models.Inventory> Save(SaveCommand saveCommand, CancellationToken cancellationToken);
    }
}
