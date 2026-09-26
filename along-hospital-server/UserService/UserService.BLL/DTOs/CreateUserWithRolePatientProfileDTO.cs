using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.DateAttributes;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;

namespace UserSvc.BLL.DTOs
{
    public class CreateUserWithRolePatientProfileDTO
    {
        public string? Name { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        [PhoneNumberValidator]
        public string? Phone { get; set; }
    }
}
