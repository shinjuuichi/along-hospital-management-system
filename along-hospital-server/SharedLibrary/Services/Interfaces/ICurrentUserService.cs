using SharedLibrary.Enums;

namespace SharedLibrary.Services.Interfaces
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        int AuthId { get; }

        RoleEnum Role { get; }

        string? Stage { get; }

        bool IsAuthenticated { get; }
    }
}