using Inventory.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Contracts;

public interface ILedgerDbContext : IDbContext
{
    DbSet<Models.Account>? Accounts { get; set; }
    DbSet<Models.Ledger>? Ledgers { get; set; }
}
