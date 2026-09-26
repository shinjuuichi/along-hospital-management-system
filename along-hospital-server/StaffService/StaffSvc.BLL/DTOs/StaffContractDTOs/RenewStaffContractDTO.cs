using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffContractDTOs
{
    public class RenewStaffContractDTO : MapTo<StaffContract>
    {
        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
    }
}
