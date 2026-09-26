using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.StaffContracts;
using MessageBroker.Contracts.StaffRequestContracts;
using MessageBroker.Events.PaymentEvents;
using MessageBroker.Events.UserEvents;
using PayrollSvc.BLL.DTOs.GlobalTaxConfigDTOs;
using PayrollSvc.BLL.DTOs.PayrollDTOs;
using PayrollSvc.BLL.DTOs.TaxBracketDTOs;
using PayrollSvc.BLL.FilterDTOs;
using PayrollSvc.DAL.Models.Snapshot;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace PayrollSvc.BLL
{
    public class PayrollMappingProfile : BaseMappingProfile
    {
        public PayrollMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            MappingGlobalTaxConfig();
            MappingRegionalWage();
            MappingStaffContract();
            MappingStaff();
            MappingSalaryAdvance();
            MappingTaxBracket();
            MappingUserFilter();
            MappingUserData();
            MappingPayment();
        }

        private void MappingGlobalTaxConfig()
        {
            CreateMap<GetGlobalTaxConfigDTO, GlobalTaxConfigSnapshot>();
        }

        private void MappingStaffContract()
        {
            CreateMap<GetStaffContractByStaffIdContract, StaffContractSnapshot>();
        }

        private void MappingStaff()
        {
            CreateMap<GetStaffProfileContract, StaffSnapshot>();
        }

        private void MappingSalaryAdvance()
        {
            CreateMap<GetUndisbursedSalaryAdvanceByStaffIdContract, SalaryAdvanceSnapshot>();
        }

        private void MappingRegionalWage()
        {
            CreateMap<GetStaffContractByStaffIdContract, RegionalWageSnapshot>()
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.RegionalWageCode))
                .ForMember(dest => dest.MonthlyWage, opt => opt.MapFrom(src => src.RegionalWageMonthlyWage));
        }

        private void MappingTaxBracket()
        {
            CreateMap<GetTaxBracketDTO, TaxBracketSnapshot>();
        }

        private void MappingUserFilter()
        {
            CreateMap<PayrollFilterDTO, GetListUserIdByFilterUserEntityEvent>();
        }

        private void MappingUserData()
        {
            CreateMap<GetUserDataByUserIdContract, DTOs.PayrollDTOs.GetPayrollDTO>()
                .ForMember(dest => dest.StaffName, opt => opt.MapFrom(src => src.Name));
        }

        private void MappingPayment()
        {
            CreateMap<PaymentStatusChangedEvent, PaymentStatusChangedDTO>();
        }
    }
}
