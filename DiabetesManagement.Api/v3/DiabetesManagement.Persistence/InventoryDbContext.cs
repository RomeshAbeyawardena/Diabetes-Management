using DiabetesManagement.Contracts;
using DiabetesManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Persistence;

public class InventoryDbContext : DbContext, IInventoryDbContext
{
    public InventoryDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {

    }

    public DbSet<Application>? Applications { get; set; }
    public DbSet<AccessToken>? AccessTokens { get; set; }
    public DbSet<AccessTokenClaim>? Claims { get; set; }
    public DbSet<Inventory>? Inventories { get; set; }
    public DbSet<InventoryHistory>? InventoryHistory { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<Session>? Sessions { get; set; }
}
