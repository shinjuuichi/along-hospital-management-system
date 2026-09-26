using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using VoucherSvc.BLL;
using VoucherSvc.DAL.Data;
using VoucherSvc.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var configuration = builder.Configuration.Get<AppConfiguration>()!;
builder.Services.AddSingleton(configuration);

builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddUploadService(configuration);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddMessageBroker(configuration, assembly: typeof(Program).Assembly);
builder.Services.AddSecurityServices(configuration);
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddMiddlewares();

builder.Services.AddHostedService<VoucherExpirationBackgroundService>();

var app = builder.Build();

await app.EnsureMongoDbCreatedAsync<VoucherDbContext>();

app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.MapDefaultEndpoints();

app.Run();
