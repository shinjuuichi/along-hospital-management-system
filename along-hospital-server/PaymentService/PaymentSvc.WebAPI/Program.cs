using PaymentSvc.BLL;
using PaymentSvc.DAL.Data;
using PaymentSvc.WebAPI.Services;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.Get<AppConfiguration>()!;

builder.AddServiceDefaults();

builder.Services.AddSingleton(configuration);

builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddUploadService(configuration);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddSecurityServices(configuration);
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddMiddlewares();
builder.Services.AddMessageBroker(configuration, typeof(Program).Assembly);

builder.Services.AddHostedService<PaymentExpiredBackgroundService>();

var app = builder.Build();

await app.EnsureMongoDbCreatedAsync<PaymentDbContext>();

app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.MapDefaultEndpoints();

app.Run();