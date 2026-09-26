using QueueSvc.BLL;
using QueueSvc.BLL.Interfaces;
using QueueSvc.DAL.Data;
using QueueSvc.WebAPI.Hubs;
using QueueSvc.WebAPI.Services;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var configuration = builder.Configuration.Get<AppConfiguration>()!;
builder.Services.AddSingleton(configuration);

builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddSecurityServices(configuration);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddMessageBroker(configuration, typeof(Program).Assembly);
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddMiddlewares();

builder.Services.AddSignalR();
builder.Services.AddScoped<IQueueHubService, QueueHubService>();
builder.Services.AddHostedService<RunAppointmentQueueingBackgroundService>();

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<QueueDbContext>();

app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.MapHub<QueueHub>("/hub/queue");
app.MapDefaultEndpoints();

app.Run();
