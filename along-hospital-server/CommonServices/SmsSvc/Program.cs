using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using SmsSvc.Services;
using SmsSvc.SmsHandler;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var configuration = builder.Configuration.Get<AppConfiguration>()!;
builder.Services.AddSingleton(configuration);

builder.Services.AddMessageBroker(configuration, assembly: typeof(Program).Assembly);

builder.Services.AddScoped<ISmsSender, TwilioSmsSender>();
builder.Services.AddScoped<ISmsService, SmsService>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
