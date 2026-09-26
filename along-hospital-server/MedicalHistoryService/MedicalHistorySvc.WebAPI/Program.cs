using MedicalHistorySvc.BLL;
using MedicalHistorySvc.DAL.Data;
using MedicalHistorySvc.WebAPI.Services;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.Get<AppConfiguration>()!;

builder.AddServiceDefaults();
builder.Services.AddSingleton(configuration);
builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddMiddlewares();
builder.Services.AddUploadService(configuration);
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddSecurityServices(configuration);
builder.Services.AddMessageBroker(configuration, typeof(Program).Assembly);
builder.Services.AddInfrastructureServices(configuration);

builder.Services.AddHostedService<TypePredictionModelRetrainingBackgroundService>();
builder.Services.AddHostedService<GenerateComplaintSummaryForLastWeekBackgroundService>();
builder.Services.AddHostedService<CancelPendingPaymentMedicalHistoryBackgroundService>();

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<MedicalHistoryDbContext>();
app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.MapDefaultEndpoints();

app.Run();
