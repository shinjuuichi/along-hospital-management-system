namespace SharedLibrary.Services.Interfaces;

public interface ITokenBlacklistService
{
    Task BlacklistAsync(string accessToken, DateTime? expiresAt = null);
    Task<bool> IsBlacklistedAsync(string accessToken);
    Task BlacklistAuthAccountAsync(int authAccountId);
    Task<bool> IsAuthAccountBlacklistedAsync(int authAccountId);
}
