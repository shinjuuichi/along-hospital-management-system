using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.DateAttributes;
using SharedLibrary.Enums;
using UserSvc.DAL.Enums;

namespace UserSvc.DAL.Models
{
    public class User : EntityWithImage
    {
        [MessageRequired]
        [MessageMaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public RoleEnum Role { get; set; } = RoleEnum.Patient;

        [MessageRequired]
        [AgeRange]
        public DateOnly DateOfBirth { get; set; }

        public GenderEnum Gender { get; set; } = GenderEnum.Other;

        [MessageMaxLength(255)]
        public string? Address { get; set; }
    }
}