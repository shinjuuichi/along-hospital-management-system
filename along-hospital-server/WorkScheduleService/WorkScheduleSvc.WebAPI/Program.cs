using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using WorkScheduleSvc.BLL;
using WorkScheduleSvc.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var configuration = builder.Configuration.Get<AppConfiguration>()!;
builder.Services.AddSingleton(configuration);

builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddSecurityServices(configuration);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddMessageBroker(configuration, assembly: typeof(Program).Assembly);
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddMiddlewares();

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<WorkScheduleDbContext>();

app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.MapDefaultEndpoints();

app.Run();
