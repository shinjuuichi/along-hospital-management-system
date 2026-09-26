using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.StaffContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Events.AuthAccountEvents;
using MessageBroker.Events.AuthAccountEvents.CreateAccountEvents;
using MessageBroker.Events.AuthAccountEvents.UpdateAccountEvents;
using MessageBroker.Events.UserEvents;
using SharedLibrary.Base.Mappers;
using StaffSvc.BLL.DTOs.SpecialtyDTOs;
using StaffSvc.BLL.DTOs.StaffAccountDTOs;
using StaffSvc.BLL.DTOs.StaffContractDTOs;
using StaffSvc.BLL.DTOs.StaffGroupDTOs;
using StaffSvc.BLL.FilterDTOs;
using StaffSvc.DAL.Models;
using System.Reflection;

namespace StaffSvc.BLL
{
    public class StaffMappingProfile : BaseMappingProfile
    {
        public StaffMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            MappingStaff();
            MappingSpecialty();
            MappingStaffGroup();
            MappingStaffContract();
        }

        private void MappingStaff()
        {
            //Event to Event
            CreateMap<CreateStaffToUserToAuthEvent, CreateUserToAuthEvent>();
            CreateMap<UpdateStaffToUserToAuthEvent, UpdateUserToAuthEvent>();

            //Contract to Contract
            CreateMap<GetUserDataByUserIdContract, GetStaffDataByUserIdContract>();

            //DTO To Event
            CreateMap<CreateStaffAndAccountDTO, CreateUserToAuthEvent>();
            CreateMap<UpdateStaffAndAccountDTO, UpdateUserToAuthEvent>();
            CreateMap<StaffFilterDTO, GetListUserIdByFilterAuthAccountEntityEvent>();
            CreateMap<StaffFilterDTO, GetListUserIdByFilterUserEntityEvent>();

            //DTO to DTO
            CreateMap<CreateStaffAndAccountDTO, GetStaffAndAccountDTO>();
            CreateMap<UpdateStaffAndAccountDTO, GetStaffAndAccountDTO>();

            //Contract to DTO
            CreateMap<GetStaffDataByUserIdContract, GetStaffAndAccountDTO>();
            CreateMap<GetUserDataByUserIdContract, GetStaffAndAccountDTO>();
            CreateMap<GetUserDataByUserIdContract, GetStaffProfileDTO>();

            //DTO to Contract
            CreateMap<GetStaffAndAccountDTO, GetStaffProfileContract>();
            CreateMap<GetStaffAndAccountDTO, GetStaffDataByUserIdContract>();
            CreateMap<GetStaffContractDTO, GetStaffContractByStaffIdContract>();
            CreateMap<GetStaffAndAccountDTO, GetStaffContractByStaffIdContract>();
        }

        private void MappingSpecialty()
        {
            //DTO to Contract
            CreateMap<GetSpecialtyDTO, GetSpecialtyByIdContract>();
            CreateMap<GetSpecialtyDTO, GetAllSpecialtiesContractItem>();
        }

        private void MappingStaffGroup()
        {
            CreateMap<GetStaffGroupDTO, GetStaffGroupByIdContract>();
            CreateMap<GetStaffGroupMemberDTO, GetStaffGroupByIdContract.StaffGroupMemberContractItem>();
        }

        private void MappingStaffContract()
        {
            CreateMap<CreateStaffContractDTO, StaffContract>()
                .ForMember(dest => dest.ContractCode, opt => opt.Ignore())
                .ForMember(dest => dest.SignatureImage, opt => opt.Ignore());
        }
    }
}
