using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Enums;

namespace MedicalServiceSvc.DAL.Models
{
    [PrimaryKey(nameof(MedicalServiceId), nameof(Role))]
    public class MedicalServiceRole : Entity
    {
        public int MedicalServiceId { get; set; }

        public RoleEnum Role { get; set; }

        public virtual MedicalService? MedicalService { get; set; }
    }
}