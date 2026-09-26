using ChatboxSvc.WebAPI;
using ChatboxSvc.WebAPI.Services;
using ChatboxSvc.WebAPI.SignalRHubs;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.Get<AppConfiguration>()!;

builder.AddServiceDefaults();
builder.Services.AddSingleton(configuration);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddMessageBroker(configuration, typeof(Program).Assembly);
builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddMiddlewares();
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddSecurityServices(configuration);

builder.Services.AddHostedService<RetrainRagContextBackgroundService>();

var app = builder.Build();

app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();
app.MapHub<ChatBotHub>("/hub/chatbot");

app.MapDefaultEndpoints();

app.Run();