using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using TeleHealthSvc.BLL;
using TeleHealthSvc.DAL.Data;
using TeleHealthSvc.WebAPI.SignalRHubs;

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
builder.Services.AddSignalR();

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<TeleHealthDbContext>();

app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();
app.MapHub<MeetingHub>("/hubs/meeting");

app.MapDefaultEndpoints();

app.Run();