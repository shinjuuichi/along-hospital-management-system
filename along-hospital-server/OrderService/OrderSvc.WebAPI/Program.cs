using OrderSvc.BLL;
using OrderSvc.DAL.Data;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.Get<AppConfiguration>()!;

builder.AddServiceDefaults();
builder.Services.AddSingleton(configuration);
builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddMiddlewares();
builder.Services.AddUploadService(configuration);
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddSecurityServices(configuration);
builder.Services.AddMessageBroker(configuration, typeof(Program).Assembly);
builder.Services.AddInfrastructureServices(configuration);

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<OrderDbContext>();
app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.MapDefaultEndpoints();

app.Run();