using DiabetesManagement.Shared.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Shared.Base
{
    public class RepositoryBase<TModel> : IRepository<TModel>
        where TModel : class
    {
        public RepositoryBase(InventoryDbContext inventoryDbContext)
        {
            Context = inventoryDbContext;
            DbSet = inventoryDbContext.Set<TModel>();
        }

        public InventoryDbContext Context { get; }

        public DbSet<TModel> DbSet { get; }
    }
}
