using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Contracts;

public interface IRepository<TDbContext, T>
    where T: class
{
    TDbContext Context { get; }
    DbSet<T> DbSet { get; }
}
