using Inventory.Base;
using Inventory.WebApi.Filters;
using LinqKit;
using Serilog;
using System.Reflection;

namespace Inventory.WebApi;

public class WebApiModule : ModuleBase
{
    public WebApiModule(Serilog.ILogger logger)
        : base(logger, nameof(WebApiModule), GetAssembly<WebApiModule>())
    {

    }

    public override bool CanRun()
    {
        return true;
    }

    public override void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var mvc = services
            .AddCors(s => s.AddDefaultPolicy(c => c
                .AllowCredentials()
                .AllowAnyHeader()
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()))
            .AddHttpContextAccessor()
            .AddResponseCaching()
            .AddControllers(options =>
            {
                var filters = options.Filters;
                filters.Add<CheckSessionFilter>();
                filters.Add<CheckFunctionFilter>();
                filters.Add<HandledExceptionFilter>();
            });

        assemblies.ForEach(a => mvc.AddApplicationPart(a));

        mvc.AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
    }
}
