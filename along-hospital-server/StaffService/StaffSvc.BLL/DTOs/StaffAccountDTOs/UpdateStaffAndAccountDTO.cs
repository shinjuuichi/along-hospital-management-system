using Microsoft.AspNetCore.Http;
using SharedLibrary.Base.Mappers;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.DateAttributes;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;
using SharedLibrary.Enums;
using StaffSvc.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace StaffSvc.BLL.DTOs.StaffAccountDTOs
{
    public class UpdateStaffAndAccountDTO : MapTo<Staff>
    {
        //Staff properties
        public int QualificationId { get; set; }

        public int SpecialtyId { get; set; }

        public string? BankCode { get; set; }

        public string? AccountNumber { get; set; }

        public int DependentQuantity { get; set; }

        //User properties
        public string? Role { get; set; }

        public string? Name { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFile? Image { get; set; }

        [AgeRange(Min = 18)]
        public DateOnly DateOfBirth { get; set; }

        //Auth properties
        [PhoneNumberValidator, MessageRequired]
        public string? Phone { get; set; }

        [EmailAddress, MessageRequired]
        public string? Email { get; set; }
    }
}
