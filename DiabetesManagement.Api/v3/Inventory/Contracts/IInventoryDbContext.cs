using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Contracts;

public interface IInventoryDbContext : IDbContext
{
    DbSet<Application>? Applications { get; set; }
    DbSet<ApplicationInstance>? ApplicationsInstance { get; set; }
    DbSet<AccessToken>? AccessTokens { get; set; }
    DbSet<AccessTokenClaim>? Claims { get; set; }
    DbSet<Models.Inventory>? Inventories { get; set; }
    DbSet<InventoryHistory>? InventoryHistory { get; set; }
    DbSet<User>? Users { get; set; }
    DbSet<Session>? Sessions { get; set; }
}
