﻿using Inventory.Base;
using Inventory.WebApi.Filters;

namespace Inventory.WebApi;

public class WebApiModule : ModuleBase
{
    public WebApiModule()
        : base(nameof(WebApiModule), GetAssembly<WebApiModule>())
    {

    }

    public override bool CanRun()
    {
        return true;
    }

    public override void RegisterServices(IServiceCollection services)
    {
        services
            .AddCors(s => s.AddDefaultPolicy(c => c
                .AllowCredentials()
                .AllowAnyHeader()
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()))
            .AddHttpContextAccessor()
            .AddResponseCaching()
            .AddControllers(options => options.Filters.Add<CheckFunctionFilter>());
    }
}
