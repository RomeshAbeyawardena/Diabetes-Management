using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Shared.Contracts
{
    public interface IRepository<TModel>
        where TModel : class
    {
        InventoryDbContext Context { get; }
        DbSet<TModel> DbSet { get; }
    }
}
