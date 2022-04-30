using Inventory.Base;
using Inventory.Core.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Inventory.Core;
public class CoreModule : ModuleBase
{
    public CoreModule(ILogger logger)
        : base(logger, nameof(CoreModule), GetAssembly<CoreModule>())
    {
    }

    public override bool CanRun()
    {
        return true;
    }

    public override void RegisterServices(IServiceCollection services)
    {
        services
            .AddSingleton<ISystemClock, SystemClock>()
            .AddSingleton<ApplicationSettings>()
            .AddTransient(typeof(MediatR.Pipeline.IRequestPreProcessor<>), typeof(AuthenticationRequestHandler<>));
    }
}
