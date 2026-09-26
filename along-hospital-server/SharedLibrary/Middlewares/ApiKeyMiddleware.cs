using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Commons;

namespace SharedLibrary.Middlewares;

public class ApiKeyMiddleware(RequestDelegate next)
{
    private const string ApiKey = "ApiKey ";

    public async Task InvokeAsync(HttpContext context, AppConfiguration appConfiguration)
    {
        var endpoint = context.GetEndpoint();
        var hasAuthorize = endpoint?.Metadata.GetMetadata<IAuthorizeData>() != null;

        if (!hasAuthorize)
        {
            await next(context);
            return;
        }

        var authHeader = context.Request.Headers.Authorization.ToString();

        if (!authHeader.StartsWith(ApiKey, StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }

        var apiKey = authHeader[ApiKey.Length..].Trim();
        var expectedApiKey = appConfiguration.SePayConfig.ApiKeyValidator;

        if (!string.Equals(apiKey, expectedApiKey, StringComparison.Ordinal))
        {
            throw new UnauthorizedAccessException("Invalid API key");
        }

        await next(context);
    }
}