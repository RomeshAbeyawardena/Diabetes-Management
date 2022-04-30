using Ledger.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Persistence;

public class LedgerDbContext : DbContext, ILedgerDbContext
{
    public LedgerDbContext(DbContextOptions options)
        : base(options)
    {

    }

    public DbSet<Models.Account>? Accounts { get; set; }
    public DbSet<Models.Ledger>? Ledgers { get; set; }
}
