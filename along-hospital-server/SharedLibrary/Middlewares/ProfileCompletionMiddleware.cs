using Microsoft.AspNetCore.Http;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;

namespace SharedLibrary.Middlewares
{
    public class ProfileCompletionMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
        {
            if (currentUserService.IsAuthenticated)
            {
                var isPending = Enum.TryParse<AuthStageEnum>(currentUserService.Stage, true, out var stage)
                    && (stage == AuthStageEnum.PatientProfilePendingWithPhone || stage == AuthStageEnum.PatientProfilePendingWithoutPhone);

                var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;

                if (isPending)
                {
                    var allowedPaths = new[]
                     {
                        "/api/v1/user/complete-profile",
                    };

                    if (!path.StartsWith("/api/v1/auth")
                        && !allowedPaths.Any(p => path.Equals(p, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw new UnauthorizedAccessException("Please complete your profile to access this feature");
                    }
                }
                else
                {
                    if (path.Equals("/api/v1/user/complete-profile", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new UnauthorizedAccessException("You have already completed your profile");
                    }
                }
            }

            await next(context);
        }
    }
}
