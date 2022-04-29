using Inventory.Contracts;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence;

public class InventoryDbContext : DbContext, IInventoryDbContext
{
    public InventoryDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {

    }

    public DbSet<Application>? Applications { get; set; }
    public DbSet<ApplicationInstance>? ApplicationInstance { get; set; }
    public DbSet<AccessToken>? AccessTokens { get; set; }
    public DbSet<AccessTokenClaim>? Claims { get; set; }
    public DbSet<Models.Inventory>? Inventories { get; set; }
    public DbSet<InventoryHistory>? InventoryHistory { get; set; }
    public DbSet<Function>? Functions { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<Session>? Sessions { get; set; }
}
