﻿using Microsoft.EntityFrameworkCore;

namespace Inventory.Contracts;

public interface IDbContextProvider
{
    IDbContext? GetDbContext(Type dbContextType);
    T? GetDbContext<T>()
        where T : IDbContext;
}