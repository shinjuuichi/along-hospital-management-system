using AuthSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;
using SharedLibrary.Enums;

namespace AuthSvc.DAL.Models;

public class AuthAccount : AuditEntity
{
    public ProviderEnum Provider { get; set; } = ProviderEnum.Phone;

    public string ProviderUserId { get; set; } = string.Empty;

    [Unique, EmailValidator, MessageMaxLength(255)]
    public string? Email { get; set; }

    [Unique, PhoneNumberValidator, MessageMaxLength(15)]
    public string? Phone { get; set; }

    [MessageRequired, PasswordValidator, MessageMaxLength(255)]
    public string Password { get; set; } = string.Empty;

    public AuthStatusEnum Status { get; set; } = AuthStatusEnum.PendingVerification;

    [Unique]
    public int? UserId { get; set; }

    public AuthStageEnum Stage { get; set; } = AuthStageEnum.Done;

    [OnDelete(OnDeleteBehavior.Cascade)]
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
