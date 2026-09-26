using PayrollSvc.BLL.DTOs.AllowanceDTOs;
using PayrollSvc.BLL.DTOs.DeductionDTOs;
using PayrollSvc.BLL.DTOs.SnapshotDTOs.GlobalTaxConfigSnapshotDTOs;
using PayrollSvc.BLL.DTOs.SnapshotDTOs.RegionalWageSnapshotDTOs;
using PayrollSvc.BLL.DTOs.SnapshotDTOs.SalaryAdvanceSnapshotDTOs;
using PayrollSvc.BLL.DTOs.SnapshotDTOs.StaffContractSnapshotDTOs;
using PayrollSvc.BLL.DTOs.SnapshotDTOs.StaffSnapshotDTOs;
using PayrollSvc.BLL.DTOs.SnapshotDTOs.TaxBracketSnapshotDTOs;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.PayrollDTOs
{
    public class GetPayrollDTO : MapFrom<Payroll>
    {
        #region Primary properties
        public int Id { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int TotalWorkedMinutes { get; set; }

        public int OvertimeMinutes { get; set; }

        public int LateMinutes { get; set; }

        public int EarlyLeaveMinutes { get; set; }

        public string? Status { get; set; }

        public Guid? TransactionId { get; set; }

        public string? QrCode { get; set; }

        public int StaffId { get; set; }

        public string? StaffName { get; set; }

        public List<GetAllowanceDTO> Allowances { get; set; } = [];

        public List<GetDeductionDTO> Deductions { get; set; } = [];
        #endregion

        #region Snapshot properties
        public GetGlobalTaxConfigSnapshotDTO? GlobalTaxConfigSnapshot { get; set; }

        public GetRegionalWageSnapshotDTO? RegionalWageSnapshot { get; set; }

        public GetStaffContractSnapshotDTO? StaffContractSnapshot { get; set; }

        public GetStaffSnapshotDTO? StaffSnapshot { get; set; }

        public GetSalaryAdvanceSnapshotDTO? SalaryAdvanceSnapshot { get; set; }

        public List<GetTaxBracketSnapshotDTO> TaxBracketSnapshot { get; set; } = [];
        #endregion

        #region Computed properties
        public double TotalAllowance { get; set; }

        public double TotalDeduction { get; set; }

        public double SalaryAdvanceAmount { get; set; }

        public double BaseSalary { get; set; }

        public double GrossSalary { get; set; }

        public double NetSalary { get; set; }

        public double SocialInsurance { get; set; }

        public double HealthInsurance { get; set; }

        public double UnemploymentInsurance { get; set; }

        public double TotalInsurance { get; set; }

        public double TotalAllowanceNotTaxable { get; set; }

        public double TaxableIncome { get; set; }

        public double AssessableIncome { get; set; }

        public double PersonalIncomeTax { get; set; }
        #endregion
    }
}
