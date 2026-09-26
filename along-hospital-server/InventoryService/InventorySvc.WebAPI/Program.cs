using InventorySvc.BLL;
using InventorySvc.DAL.Data;
using InventorySvc.WebAPI.Services;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
var configuration = builder.Configuration.Get<AppConfiguration>()!;
builder.Services.AddSingleton(configuration);

builder.Services.AddInfrastructureServices(configuration);

builder.Services.AddMessageBroker<InventoryDbContext>(configuration, assembly: typeof(Program).Assembly);

builder.Services.AddBaseServices();

builder.Services.AddUploadService(configuration);

builder.Services.AddSecurityServices(configuration);

builder.Services.AddDefaultAPIServices();

builder.Services.AddHostedService<LowStockInventoryBackgroundService>();

builder.Services.AddSwaggerServices(configuration);

builder.Services.AddMiddlewares();

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<InventoryDbContext>();

app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.MapDefaultEndpoints();

app.Run();
