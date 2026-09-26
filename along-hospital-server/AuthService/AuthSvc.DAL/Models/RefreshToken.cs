using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace AuthSvc.DAL.Models
{
    public class RefreshToken : AuditEntity
    {
        [MessageRequired]
        public string RefreshTokenHash { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public int AuthAccountId { get; set; }
        public virtual AuthAccount AuthAccount { get; set; } = null!;
    }
}
