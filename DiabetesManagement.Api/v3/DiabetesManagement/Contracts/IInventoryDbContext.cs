﻿using DiabetesManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Contracts;

public interface IInventoryDbContext : IDbContext
{
    DbSet<Application>? Applications { get; set; }
    DbSet<AccessToken>? AccessTokens { get; set; }
    DbSet<AccessTokenClaim>? Claims { get; set; }
    DbSet<Inventory>? Inventories { get; set; }
    DbSet<InventoryHistory>? InventoryHistory { get; set; }
    DbSet<User>? Users { get; set; }
    DbSet<Session>? Sessions { get; set; }
}
