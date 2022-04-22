using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Contracts;

public interface IDbContextProvider
{
    IDbContext? GetDbContext(Type dbContextType);
    T? GetDbContext<T>()
        where T: IDbContext;
}
