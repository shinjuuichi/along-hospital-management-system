using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.PayrollDTOs
{
    public class CreatePayrollDTO : MapTo<Payroll>
    {
        #region Primary properties
        public int TotalWorkedMinutes { get; set; }

        public int OvertimeMinutes { get; set; }

        public int LateMinutes { get; set; }

        public int EarlyLeaveMinutes { get; set; }
        #endregion

        #region Foreign keys
        public int StaffId { get; set; }
        #endregion
    }
}