using BillingSvc.BLL;
using BillingSvc.DAL.Data;
using BillingSvc.WebAPI.Services;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
var configuration = builder.Configuration.Get<AppConfiguration>()!;

builder.Services.AddSingleton(configuration);
builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddMiddlewares();
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddSecurityServices(configuration);
builder.Services.AddMessageBroker(configuration, typeof(Program).Assembly);
builder.Services.AddInfrastructureServices(configuration);

builder.Services.AddHostedService<CancelExpiredRefundBackgroundService>();

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<BillingDbContext>();
app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.MapDefaultEndpoints();

app.Run();
