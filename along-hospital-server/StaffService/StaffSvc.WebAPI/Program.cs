using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using StaffSvc.BLL;
using StaffSvc.DAL.Data;
using StaffSvc.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.Get<AppConfiguration>()!;

builder.AddServiceDefaults();
builder.Services.AddSingleton<AppConfiguration>();
builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddMiddlewares();
builder.Services.AddUploadService(configuration);
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddSecurityServices(configuration);
builder.Services.AddMessageBroker(configuration, typeof(Program).Assembly);
builder.Services.AddInfrastructureService(configuration);
builder.Services.AddHostedService<StaffCertificateExpirationBackgroundService>();
builder.Services.AddHostedService<StaffContractStatusBackgroundService>();
builder.Services.AddHostedService<StaffContractExpiringNotificationBackgroundService>();
builder.Services.AddHostedService<StaffCertificateExpirationReminderBackgroundService>();

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<StaffDbContext>();
app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.MapDefaultEndpoints();

app.Run();