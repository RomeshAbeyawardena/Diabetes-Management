using DiabetesManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {

    }

    public DbSet<Inventory>? Inventories { get; set; }
    public DbSet<InventoryHistory>? InventoryHistory { get; set; }
    public DbSet<User>? Users { get; set; }
}
