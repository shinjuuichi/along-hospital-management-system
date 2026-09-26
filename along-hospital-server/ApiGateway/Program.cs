using ApiGateway;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using SharedLibrary.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

var configuration = builder.Configuration.Get<AppConfiguration>()!;
builder.Services.AddSingleton(configuration);

builder.Services.AddApiGatewayServices(builder.Environment);
builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddSecurityServices(configuration);
builder.Services.AddRedisDatabase(configuration);
builder.Services.AddMiddlewares();

var app = builder.Build();

app.UseSecurityServices();
app.UseWebSockets();
app.UseMiddlewares();
app.UseMiddleware<JwtBlacklistMiddleware>();
app.MapDefaultEndpoints();
app.UseApiGatewayServices();
app.UseDefaultAPIServices();

app.Run();
