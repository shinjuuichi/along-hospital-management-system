using SharedLibrary.Commons;
using SharedLibrary.Enums;
using SharedLibrary.Extensions;
using UserSvc.BLL;
using UserSvc.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var configuration = builder.Configuration.Get<AppConfiguration>()!;
builder.Services.AddSingleton(configuration);

builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddUploadService(configuration);
builder.Services.AddMessageBroker<UserDbContext>(configuration, assembly: typeof(Program).Assembly);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddSecurityServices(configuration);
builder.Services.AddMiddlewares();
builder.Services.AddSwaggerServices(configuration);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ProfilePending", policy =>
        policy.RequireClaim("Stage", [nameof(AuthStageEnum.PatientProfilePendingWithPhone), nameof(AuthStageEnum.PatientProfilePendingWithoutPhone)]));

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<UserDbContext>();

app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.MapDefaultEndpoints();

app.Run();
