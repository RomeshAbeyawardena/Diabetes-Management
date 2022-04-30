using Ledger.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Persistence;

public class LedgerDbContext : DbContext, ILedgerDbContext
{
    public LedgerDbContext(DbContextOptions options)
        : base(options)
    {

    }
}
