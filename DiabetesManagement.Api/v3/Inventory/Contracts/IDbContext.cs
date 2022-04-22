using Microsoft.EntityFrameworkCore;

namespace Inventory.Contracts;

public interface IDbContext
{
    DbSet<T> Set<T>()
        where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
