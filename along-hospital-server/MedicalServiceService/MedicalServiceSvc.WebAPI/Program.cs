using MedicalServiceSvc.BLL;
using MedicalServiceSvc.DAL.Data;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

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

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<MedicalServiceDbContext>();
app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.MapDefaultEndpoints();

app.Run();
