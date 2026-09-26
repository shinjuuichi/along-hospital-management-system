using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Services.Interfaces;

namespace SharedLibrary.Middlewares;

public class JwtBlacklistMiddleware(RequestDelegate next)
{
    private static readonly PathString[] PublicAuthPathPrefixes =
    [
        new("/api/v1/auth/login"),
        new("/api/v1/auth/register"),
        new("/api/v1/auth/refresh"),
        new("/api/v1/auth/forgot-password")
    ];

    public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklistService)
    {
        if (this.ShouldSkipBlacklistCheck(context))
        {
            await next(context);
            return;
        }

        var token = this.GetAccessToken(context);
        if (!string.IsNullOrWhiteSpace(token)
            && await blacklistService.IsBlacklistedAsync(token))
        {
            throw new UnauthorizedAccessException("Token has been revoked");
        }

        var authAccountId = this.GetAuthAccountId(context);
        if (authAccountId.HasValue
            && await blacklistService.IsAuthAccountBlacklistedAsync(authAccountId.Value))
        {
            throw new UnauthorizedAccessException("Your account is no longer allowed to access the system");
        }

        await next(context);
    }

    private bool ShouldSkipBlacklistCheck(HttpContext context)
    {
        if (this.IsPublicAuthEndpoint(context.Request.Path))
        {
            return true;
        }

        var endpoint = context.GetEndpoint();
        var allowsAnonymous = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;
        var requiresAuthorization = endpoint?.Metadata.GetMetadata<IAuthorizeData>() != null;

        if (allowsAnonymous)
        {
            return true;
        }

        if (requiresAuthorization)
        {
            return false;
        }

        return !this.HasAuthenticatedPrincipal(context)
            && string.IsNullOrWhiteSpace(this.GetAccessToken(context));
    }

    private bool IsPublicAuthEndpoint(PathString path)
    {
        return PublicAuthPathPrefixes.Any(prefix => path.StartsWithSegments(prefix));
    }

    private string? GetAccessToken(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.ToString();
        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authHeader[7..];
        }

        if (!context.Request.Path.StartsWithSegments("/hubs", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var accessToken = context.Request.Query["access_token"].ToString();
        return string.IsNullOrWhiteSpace(accessToken) ? null : accessToken;
    }

    private bool HasAuthenticatedPrincipal(HttpContext context)
    {
        return context.User.Identity?.IsAuthenticated == true;
    }

    private int? GetAuthAccountId(HttpContext context)
    {
        if (!this.HasAuthenticatedPrincipal(context))
        {
            return null;
        }

        var authIdValue = context.User.FindFirst("AuthId")?.Value;
        return int.TryParse(authIdValue, out var authAccountId) ? authAccountId : null;
    }
}
