using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Contracts;

public interface IDbContext
{
    DbSet<T> Set<T>()
        where T: class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
