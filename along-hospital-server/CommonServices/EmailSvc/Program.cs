using EmailSvc.EmailHandler;
using EmailSvc.Profiles;
using EmailSvc.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
var configuration = builder.Configuration.Get<AppConfiguration>()!;
builder.Services.AddSingleton(configuration);

builder.Services.AddMessageBroker(configuration, assembly: typeof(Program).Assembly);

builder.Services.AddSingleton<ISendGridClient>(new SendGridClient(configuration.EmailConfig.ApiKey));
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddAutoMapper(typeof(EmailMappingProfile).Assembly);

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();