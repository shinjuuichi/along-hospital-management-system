using Amazon.S3;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using UploadSvc.Processing;
using UploadSvc.Services.Implements;
using UploadSvc.Services.Interfaces;
using UploadSvc.Storage;
using UploadSvc.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var configuration = builder.Configuration.Get<AppConfiguration>()!;
builder.Services.AddSingleton(configuration);

builder.Services.AddSingleton<IAmazonS3>(serviceProvider =>
{
    var config = new AmazonS3Config
    {
        ServiceURL = $"https://{configuration.R2Config.AccountId}.r2.cloudflarestorage.com",
        ForcePathStyle = true,
        SignatureMethod = Amazon.Runtime.SigningAlgorithm.HmacSHA256,
        MaxErrorRetry = 3,
        Timeout = TimeSpan.FromMinutes(5),
        AuthenticationRegion = "auto",
        BufferSize = 65536,
        DisableLogging = false,
        UseHttp = false
    };

    var credentials = new Amazon.Runtime.BasicAWSCredentials(configuration.R2Config.AccessKeyId, configuration.R2Config.SecretAccessKey);

    return new AmazonS3Client(credentials, config);
});

builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IStorageService, R2StorageService>();
builder.Services.AddScoped<IFileValidator, FileValidator>();
builder.Services.AddScoped<IFileProcessor, ImageProcessor>();
builder.Services.AddScoped<IFileProcessor, DefaultFileProcessor>();

builder.Services.AddDefaultAPIServices();
builder.Services.AddBaseServices();
builder.Services.AddSecurityServices(configuration);
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddMiddlewares();

var app = builder.Build();

app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();
app.MapDefaultEndpoints();

app.Run();
