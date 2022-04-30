using Inventory;
using Inventory.Base;
using Ledger.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Ledger.Persistence;
public class LedgerPersistenceModule : ModuleBase
{
    public LedgerPersistenceModule(ILogger logger)
        : base(logger, nameof(LedgerPersistenceModule), GetAssembly<LedgerPersistenceModule>())
    {
    }

    public override bool CanRun()
    {
        return true;
    }

    public override void RegisterServices(IServiceCollection services)
    {
        services
            .AddDbContext<LedgerDbContext>(ConfigureDbContext)
            .AddTransient<ILedgerDbContext>(s => s.GetRequiredService<LedgerDbContext>());
    }

    private static void ConfigureDbContext(IServiceProvider serviceProvider, DbContextOptionsBuilder builder)
    {
        var applicationSettings = serviceProvider.GetService<ApplicationSettings>();
        builder.UseSqlServer(applicationSettings!.DefaultConnectionString);
    }
}
