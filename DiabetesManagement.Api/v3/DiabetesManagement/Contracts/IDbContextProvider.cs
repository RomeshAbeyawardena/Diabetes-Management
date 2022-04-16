using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Contracts;

public interface IDbContextProvider
{
    DbContext? GetDbContext(Type dbContextType);
    T? GetDbContext<T>()
        where T: DbContext;
}
