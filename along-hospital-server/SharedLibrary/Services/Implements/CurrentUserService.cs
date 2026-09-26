using Microsoft.AspNetCore.Http;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;
using System.Security.Claims;

namespace SharedLibrary.Services.Implements
{
    public class CurrentUserService : ICurrentUserService
    {
        public int UserId { get; } = 0;

        public int AuthId { get; } = 0;

        public RoleEnum Role { get; }

        public string? Stage { get; }

        public bool IsAuthenticated { get; } = false;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var authIdValue = user.FindFirstValue("AuthId");

                if (int.TryParse(userIdValue, out var userId) && int.TryParse(authIdValue, out var authId))
                {
                    UserId = userId;
                    AuthId = authId;
                    IsAuthenticated = true;

                    var roleClaim = user.FindFirstValue(ClaimTypes.Role);
                    if (!string.IsNullOrEmpty(roleClaim) && Enum.TryParse<RoleEnum>(roleClaim, true, out var role))
                    {
                        Role = role;
                    }

                    Stage = user.FindFirstValue("Stage");
                }
            }
        }
    }
}
