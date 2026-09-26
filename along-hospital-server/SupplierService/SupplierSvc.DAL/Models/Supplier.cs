using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;

namespace SupplierSvc.DAL.Models
{
    public class Supplier : AuditEntity
    {
        [MessageRequired]
        [NotContainsSpecialCharacterValidator]
        [Unique]
        [MessageMaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MessageRequired]
        [PhoneNumberValidator]
        [Unique]
        [MessageMaxLength(15)]
        public string Phone { get; set; } = string.Empty;

        [MessageRequired]
        [EmailValidator]
        [Unique]
        [MessageMaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [MessageRequired]
        [MessageMaxLength(255)]
        public string Address { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? Note { get; set; }
    }
}