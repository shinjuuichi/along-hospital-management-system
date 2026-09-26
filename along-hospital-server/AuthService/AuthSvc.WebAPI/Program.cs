using AuthSvc.BLL;
using AuthSvc.DAL.Data;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using SharedLibrary.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

var configuration = builder.Configuration.Get<AppConfiguration>()!;
builder.Services.AddSingleton(configuration);

builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddSecurityServices(configuration);
builder.Services.AddMessageBroker<AuthDbContext>(configuration, assembly: typeof(Program).Assembly);
builder.Services.AddMiddlewares();
builder.Services.AddSwaggerServices(configuration);

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<AuthDbContext>();

app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseMiddleware<JwtBlacklistMiddleware>();
app.UseDefaultAPIServices();

app.MapDefaultEndpoints();

app.Run();
