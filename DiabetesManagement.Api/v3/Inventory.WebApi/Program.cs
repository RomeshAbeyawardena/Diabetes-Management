using Inventory.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .RegisterCoreServices("modules.json");

var app = builder
    .Build();

app.MapControllers();
app.UseCors();
app.Run();
