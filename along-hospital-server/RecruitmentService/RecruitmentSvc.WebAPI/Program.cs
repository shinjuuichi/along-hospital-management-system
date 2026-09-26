using RecruitmentSvc.BLL;
using RecruitmentSvc.DAL.Data;
using RecruitmentSvc.WebAPI.Services;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.Get<AppConfiguration>()!;

builder.AddServiceDefaults();
builder.Services.AddSingleton(configuration);
builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddMiddlewares();
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddSecurityServices(configuration);
builder.Services.AddMessageBroker(configuration, typeof(Program).Assembly);
builder.Services.AddUploadService(configuration);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddHostedService<JobPostingExpirationBackgroundService>();

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<RecruitmentDbContext>();
app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.MapDefaultEndpoints();

app.Run();